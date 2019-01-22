using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Windows.Input;
using WpfApplication1.Model;
using MVVM;
using System.Collections.ObjectModel;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Xml;
using WpfApplication1.View;
using XMLSerial;
namespace WpfApplication1.ViewModel
{
    // All commands here, except 

    // Note: Not very "DRY". Use MVVM framework to bind to functions by convention
    // Note: Some System.IO used here, could be moved to model
    // Note: Using cosale operator "??" saves some characters, but formatting not standard/ a little awkward

    public partial class MainVm
    {
        private int GetIndexFolderPlanes(string path)
        {
            int indexInPlanes = -1;
            for (int i = 0; i <= FolderPlanes.Count - 1; i++)
            {
                if (FolderPlanes[i].FullPathName == path) { indexInPlanes = i; }
            }
            return indexInPlanes;
        }


        RelayCommand renameCommand;
        public ICommand RenameCommand
        {
            get { return renameCommand ?? (renameCommand = new RelayCommand(x => Rename(x))); }
        }

        public void Rename(object p)
        {
            // Rename single selected folder or file
            if (SelectedFolderItems.Count != 1)
            {
                Popup3IsOpen = false;
                return;
            }
            string newname = Path.Combine(selectedFolderPlane.FullPathName, (p as String));
            string oldname = (SelectedFolderItems[0]);

            if (File.Exists(oldname) && !File.Exists(newname))
            {
                File.Copy(oldname, newname);
                File.Delete(oldname);
            }

            if (Directory.Exists(oldname) && !Directory.Exists(newname))
            {
                try
                {
                    Directory.Move(oldname, newname);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Failed to rename:  " + ex);
                    throw;
                }
            }
            RefreshFolderPlanesAndSelectedNavTree(null);
            Popup3IsOpen = false;
        }
        public void ReNameDirFolder(string name)
        {
            string newname = Path.Combine(selectedFolderPlane.FullPathName, name);
            string oldname = (SelectedFolderItems[0]);

            if (File.Exists(oldname) && !File.Exists(newname))
            {
                File.Copy(oldname, newname);
                File.Delete(oldname);
            }

            if (Directory.Exists(oldname) && !Directory.Exists(newname))
            {
                try
                {
                    Directory.Move(oldname, newname);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Failed to rename:  " + ex);
                    throw;
                }

            }

            RefreshFolderPlanesAndSelectedNavTree(null);

        }
        // Deletion from SelectedFolderItems. Alternative choice: use snapshot and delete SnappedSelectedItems //
        RelayCommand deleteSelectedCommand;
        public ICommand DeleteSelectedCommand
        {
            get
            {
                return deleteSelectedCommand ??
                       (deleteSelectedCommand = new RelayCommand(x => DeleteSelected(x), x => SelectedFolderItems.Count != 0));
            }

        }
        public void DeleteSelected(object p)
        {
            if (selectedFolderItems == null) return;
            if (selectedFolderItems.Count == 0) return;
            string str;
            MessageBoxResult dr = MessageBox.Show("是否确定删除选中的文件", "删除提示",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (dr == MessageBoxResult.OK)
            {
                for (int i = 0; i <= SelectedFolderItems.Count - 1; i++)
                {
                    str = SelectedFolderItems[i];
                    try
                    {
                        if (System.IO.Directory.Exists(str))
                        {
                            Directory.Delete(str, true);
                        }
                        else if (System.IO.File.Exists(str))
                        {
                            File.Delete(str);
                        }
                    }
                    catch
                    {
                        // if no permissions we fail and continue
                        Console.WriteLine(" Failed to delete: " + str);
                    }
                }
                RefreshFolderPlanesAndSelectedNavTree(null);
            }
            else
            {
                RefreshFolderPlanesAndSelectedNavTree(null);
                return;

            }
            // to do: test if folder can be deleted, send notification if not
            // to do: refresh all FolderPlanes
        }
        RelayCommand refreshFolderPlanesCommand;
        public ICommand RefreshFolderPlanesCommand
        {
            get { return refreshFolderPlanesCommand ?? (refreshFolderPlanesCommand = new RelayCommand(RefreshFolderPlanesAndSelectedNavTree)); }
        }

        public void RefreshFolderPlanesAndSelectedNavTree(object p)
        {
            SelectedFolderPlane.RefreshFolderPlane();


            // In RefreshFolderPlane Directory.Exists(FullPathName) is checked. If false FullPathName set to ""
            // When a SelectedFolderPlane is removed, another existing SelectedFolderPlane is chosen
            for (int i = FolderPlanes.Count - 1; i >= 0; i--)
            {
                folderPlanes[i].RefreshFolderPlane();
                if (folderPlanes[i].FullPathName == "")
                {
                    var item = folderPlanes[i];
                    folderPlanes.Remove(item);
                }
            }

            // Commented Code for testing, you can see here what folders are expanded when pressing RefreshButton
            // List<string> SnapShot = TreeUtils.TakeSnapshot(TabbedNavTrees.SelectedNavTree.RootNode);

            TabbedNavTrees.SelectedNavTree.RebuildTree();

            // When no selectedFolderPlane is selected FullPathName is null
            SelectedPath = SelectedFolderPlane.FullPathName ?? "";
        }
        // For now SelectedPath common to all trees
        // to do? set OnClickCommand as attribute to NavTreeView and TabbedNavTrees
        RelayCommand selectedPathFromTreeCommand;
        public ICommand SelectedPathFromTreeCommand
        {

            get
            {
                return selectedPathFromTreeCommand ??
                       (selectedPathFromTreeCommand =
                              new RelayCommand(x => SelectedPath = (x as string)));
            }
        }

        RelayCommand folderPlaneItemDoubleClickCommand;
        public ICommand FolderPlaneItemDoubleClickCommand
        {
            get
            {
                return folderPlaneItemDoubleClickCommand ??
                       (folderPlaneItemDoubleClickCommand =
                              new RelayCommand(x => OnFolderDownClick(x), x => true));
            }
        }

        // In case of doubleclick on FolderItem we choose/want to change current selected folder
        // We do not know the source of the Set/change event in SelectedPath
        // For now we use a boolean for this, however I am not sure if this completely (thread???) safe
        private bool UseCurrentPlane = false;

        public void OnFolderDownClick(object p)
        {
            if (p == null) return;
            string path = (p as FolderPlaneItem).FullPathName;

            bool isDrive = FolderPlaneUtils.IsDrive(path);
            bool isFolder = FolderPlaneUtils.IsFolder(path);
            if (isDrive || isFolder)
            {
                UseCurrentPlane = true;
                try
                {
                    SelectedPath = path;
                }
                finally { UseCurrentPlane = false; }
            }
            else
            {
                // Execute
                try
                {
                    // Console.WriteLine("Execute: "+ path);
                    System.Diagnostics.Process.Start(path);
                }
                catch
                { }
            }
        }

        RelayCommand closeTabCommand;
        public ICommand CloseTabCommand
        {
            get { return closeTabCommand ?? (closeTabCommand = new RelayCommand(CloseTab, x => FolderPlanes.Count > 0)); }
        }

        //public int teller = 0;
        public void CloseTab(object p)
        {
            //teller++;
            //Console.WriteLine("*********** OnCloseCommand called ************" + teller.ToString());

            if (SelectedFolderPlane != null)
            {
                int i = GetIndexFolderPlanes(SelectedFolderPlane.FullPathName);

                if (i != -1)
                {
                    FolderPlanes.RemoveAt(i);
                    if (FolderPlanes.Count != 0)
                    {
                        i = i - 1;
                        if (i < 0) i = i + 1;
                        SelectedPath = FolderPlanes[i].FullPathName;
                    }
                    else
                    {
                        SelectedFolderPlane = null;
                        SelectedPath = "";
                    }
                }
            }
        }

        RelayCommand folderUpCommand;
        public ICommand FolderUpCommand
        {
            get { return folderUpCommand ?? (folderUpCommand = new RelayCommand(FolderUp, x => FolderPlanes.Count > 0)); }
        }
        public void FolderUp(object p)
        {
            if (SelectedFolderPlane != null)
            {
                string path = FolderPlaneUtils.FolderUp(SelectedFolderPlane.FullPathName);
                UseCurrentPlane = true;
                try
                {
                    SelectedPath = path;
                }
                finally { UseCurrentPlane = false; }

            }
        }

        // Note: we now use drag and drop for ordering Folders and SavedTabs

        //RelayCommand tabToLeftCommand;
        //public ICommand TabToLeftCommand
        //{
        //    get { return tabToLeftCommand ?? (tabToLeftCommand = new RelayCommand(TabToLeft)); }
        //}

        //public void TabToLeft(object p)
        //{
        //    int index = GetIndexFolderPlanes(selectedPath);
        //    if (index > 0)
        //    {
        //        FolderPlanes.Move(index, index - 1);
        //    }
        //}

        //RelayCommand tabToRightCommand;
        //public ICommand TabToRightCommand
        //{
        //    get { return tabToRightCommand ?? (tabToRightCommand = new RelayCommand(TabToRight)); }
        //}

        //public void TabToRight(object p)
        //{
        //    int index = GetIndexFolderPlanes(selectedPath);
        //    if ((index != -1) && (index < FolderPlanes.Count - 1))
        //    {
        //        FolderPlanes.Move(index, index + 1);
        //    }
        //}

        RelayCommand toggleOpenPopup1Command;
        public ICommand ToggleOpenPopup1Command
        {
            get { return toggleOpenPopup1Command ?? (toggleOpenPopup1Command = new RelayCommand(x => Popup1IsOpen = !Popup1IsOpen)); }
        }

        RelayCommand toggleOpenPopup2Command;
        public ICommand ToggleOpenPopup2Command
        {
            get { return toggleOpenPopup2Command ?? (toggleOpenPopup2Command = new RelayCommand(x => Popup2IsOpen = !Popup2IsOpen)); }
        }

        RelayCommand toggleOpenPopup3Command;
        public ICommand ToggleOpenPopup3Command
        {
            get
            {
                return toggleOpenPopup3Command ??
                      (toggleOpenPopup3Command = new RelayCommand
                             (x => Popup3IsOpen = (!Popup3IsOpen && (SelectedFolderItems.Count == 1)),
                                                     x => SelectedFolderItem != ""));
            }
        }

        RelayCommand addSavedTabsCommand;
        public ICommand AddSavedTabsCommand
        {
            get { return addSavedTabsCommand ?? (addSavedTabsCommand = new RelayCommand(AddSavedTabs)); }
        }

        public void AddSavedTabs(object p)
        {
            string name = (p as String);

            var saveTheseTabs = new SavedFolderTabsItem() { };
            saveTheseTabs.FriendlyName = name;
            saveTheseTabs.TabFullPathName = new System.Collections.ObjectModel.Collection<string>() { };
            for (int i = 0; i <= FolderPlanes.Count - 1; i++)
            {
                saveTheseTabs.TabFullPathName.Add(FolderPlanes[i].FullPathName);
            }


            SavedFolderTabs.Add(saveTheseTabs);
            // to do: .Add does not set and save ???
            SavedFolderTabsUtils.Save(savedFolderTabs);
            Popup2IsOpen = !Popup2IsOpen;
        }

        RelayCommand deleteSavedTabsCommand;
        public ICommand DeleteSavedTabsCommand
        {
            get { return deleteSavedTabsCommand ?? (deleteSavedTabsCommand = new RelayCommand(DeleteSavedTabs)); }
        }

        public void DeleteSavedTabs(object p)
        {
            int index = (int)p;
            if (index != -1) { SavedFolderTabs.RemoveAt(index); }
            SavedFolderTabsUtils.Save(savedFolderTabs);
            Popup1IsOpen = !Popup1IsOpen;
        }

        RelayCommand closeAllFolderTabsCommand;
        public ICommand CloseAllFolderTabsCommand
        {
            get
            {
                return closeAllFolderTabsCommand ??
                       (
                        closeAllFolderTabsCommand = new RelayCommand(x =>
                          {
                              SelectedPath = "";
                              SelectedFolderPlane = null;
                              FolderPlanes.Clear();
                              SelectedIndexSavedFolderTabs = -1;
                          })
                       );
            }
        }

        RelayCommand selectedItemsChangedCommand;
        public ICommand SelectedItemsChangedCommand
        {
            get { return selectedItemsChangedCommand ?? (selectedItemsChangedCommand = new RelayCommand(SelectedItemsChanged, x => true)); }
        }

        public void SelectedItemsChanged(object p)
        {
            SelectedFolderItems.Clear();

            IList selectedRecords = p as IList;
            foreach (FolderPlaneItem item in selectedRecords)
            {
                SelectedFolderItems.Add(item.FullPathName);
            }

            if (SelectedFolderItems.Count == 1) { SelectedFolderItem = Path.GetFileName(SelectedFolderItems[0]); } else { SelectedFolderItem = ""; }
        }

        RelayCommand snapShotSelectedCommand;
        public ICommand SnapShotSelectedCommand
        {
            get
            {
                return snapShotSelectedCommand ??
                       (snapShotSelectedCommand = new RelayCommand(SnapShotSelected, x => SelectedFolderItems.Count != 0));
            }
        }

        public void SnapShotSelected(object p)
        {
            SnappedSelectedItems.Clear();
            foreach (string item in SelectedFolderItems)
            {
                SnappedSelectedItems.Add(item);
            }
        }

        RelayCommand copySnapShotCommand;
        public ICommand CopySnapShotCommand
        {
            get
            {
                return copySnapShotCommand ??
                       (copySnapShotCommand = new RelayCommand(CopySnapShot, x => SnappedSelectedItems.Count != 0));
            }

        }
        public void CopySnapShot(object p) { CopyMoveSnapShot(p); }

        RelayCommand moveSnapShotCommand;
        public ICommand MoveSnapShotCommand
        {
            get
            {
                return moveSnapShotCommand ??
                       (moveSnapShotCommand = new RelayCommand(MoveSnapShot, x => SnappedSelectedItems.Count != 0));
            }

        }
        public void MoveSnapShot(object p) { CopyMoveSnapShot(p, false); }

        RelayCommand copySnapShotAddDateCommand;
        public ICommand CopySnapShotAddDateCommand
        {
            get
            {
                return copySnapShotAddDateCommand ??
                       (copySnapShotAddDateCommand = new RelayCommand(CopySnapShotAddDate, x => SnappedSelectedItems.Count == 1));
            }

        }
        public void CopySnapShotAddDate(object p) { CopyMoveSnapShot("Date"); }

        // http://stackoverflow.com/questions/627504/what-is-the-best-way-to-recursively-copy-contents-in-c
        // Note filemanagement not ready. No feedback. UI hangs on large files, must in other process (Backgroundworker?)
        // to do: to model
        public void CopyDirAndChildren(DirectoryInfo source, DirectoryInfo target)
        {
            // Saw once error during heavy copying, not reproducable, new folder/copy to .exe folder 
            if ((target.Parent == null) || (target.Parent.FullName == "")) return;


            // Deny action if target is a (indirect) child of target, recursive loops
            // Test target startswith sourcefullname
            // can be longer folder/file name, so extra test on common parent
            if ((target.FullName.StartsWith(source.FullName)) && (target.Parent.FullName != source.Parent.FullName)) return;

            try
            {
                //check if the target directory exists
                if (Directory.Exists(target.FullName) == false)
                {
                    Directory.CreateDirectory(target.FullName);
                }

                //copy all the files into the new directory

                foreach (FileInfo fi in source.GetFiles())
                {
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }


                //copy all the sub directories using recursion

                foreach (DirectoryInfo diSourceDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetDir = target.CreateSubdirectory(diSourceDir.Name);
                    CopyDirAndChildren(diSourceDir, nextTargetDir);
                }
                //success here
            }
            catch //(IOException ie) 
            {
                //handle it here
            }

        }

        public void CopyMoveSnapShot(object p, bool CopyNotMove = true)
        {
            // Ctrl+d pressed and 1 item in snapshot
            // to do: make a extra command for it, work with SelectedFolderItems (items=1)
            string extra = "";
            if ((p as string) == "Date")
            {
                if (SnappedSelectedItems.Count != 1) return;

                DateTime time = DateTime.Now;
                string format = " yyyy-M-d HH.mm -)";
                extra = time.ToString(format);
            }

            // Saw once error during heavy copying, not reproducable, new folder/copy to .exe folder 
            if ((SelectedFolderPlane == null) || (SelectedFolderPlane.FullPathName == "") || (SelectedPath == "")) return;


            // Copy SelectedFolderItems to SelectedFolderPlane.FullName
            string targetPath = SelectedFolderPlane.FullPathName;
            if (!System.IO.Directory.Exists(targetPath)) return;

            string sourcePath = "";
            string fileName = "";

            foreach (string fullPathName in SnappedSelectedItems)
            {
                sourcePath = Path.GetDirectoryName(fullPathName);
                fileName = Path.GetFileName(fullPathName);

                if (System.IO.Directory.Exists(fullPathName))
                {
                    // construct targetItem, if exists try -copy ()
                    string targetItem = Path.Combine(targetPath, fileName + extra);

                    int i = 0;
                    string targetItemCopy = targetItem + " - copy(";
                    while (System.IO.Directory.Exists(targetItem) && (i < 100))
                    {
                        targetItem = targetItemCopy + i.ToString() + ")";
                        i++;
                    }

                    if (CopyNotMove)
                    {
                        CopyDirAndChildren(new DirectoryInfo(fullPathName), new DirectoryInfo(targetItem));
                    }
                    else
                    {
                        Directory.Move(fullPathName, targetItem);
                    }
                }
                else if (System.IO.File.Exists(fullPathName))
                {
                    // construct targetItem, if exists try -copy ()
                    string targetItem = Path.Combine(targetPath, fileName);

                    int i = 0;
                    string targetItemCopy = Path.Combine(targetPath, Path.GetFileNameWithoutExtension(fileName)) + " - copy(";
                    string targetExt = Path.GetExtension(fileName);

                    while (System.IO.File.Exists(targetItem) && (i < 100))
                    {
                        targetItem = targetItemCopy + i.ToString() + ")" + targetExt;
                        i++;
                    }

                    // error can happen
                    File.Copy(fullPathName, targetItem, true);
                    if (!CopyNotMove) File.Delete(fullPathName);
                }
            }

            if (!CopyNotMove) SnappedSelectedItems.Clear();
            RefreshFolderPlanesAndSelectedNavTree(null);
        }
        RelayCommand createNewEmptyFolder;
        public ICommand CreateNewEmptyFolder
        {
            // test CanExecute not correct hack, to do?
            get { return createNewEmptyFolder ?? (createNewEmptyFolder = new RelayCommand(NewEmptyFolder, x => selectedFolderPlane.FullPathName != null)); }
        }
        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="p"></param>
        public void NewEmptyFolder(object p)
        {
            // Saw once error during heavy copying, not yet reproducable, new folder/copy to .exe folder 
            if ((SelectedFolderPlane == null) ||
                (SelectedFolderPlane.FullPathName == "") ||
                (SelectedPath == "")) return;

            string targetPath = SelectedFolderPlane.FullPathName;

            if (!System.IO.Directory.Exists(targetPath)) return;
            string newPath = System.IO.Path.Combine(targetPath, "新建文件夹");
            //string sub = newPath.Substring(0);
            //string path = System.IO.Path.Combine(sub, "FinFileAttri");
            int i = 0;
            while ((System.IO.Directory.Exists(newPath)) && (i < 100))
            {
                i++;
                newPath = System.IO.Path.Combine(targetPath,
                    "新建文件夹" + (i.ToString()));
                // CreateXmlFile(newPath);
            }
            System.IO.Directory.CreateDirectory(newPath);
            RefreshFolderPlanesAndSelectedNavTree(null);
        }

        private string NewObjFolder(string foldername)
        {
            // Saw once error during heavy copying, not yet reproducable, new folder/copy to .exe folder 
            if ((SelectedFolderPlane == null) ||
                (SelectedFolderPlane.FullPathName == "") ||
                (SelectedPath == "")) return null;

            string targetPath = SelectedFolderPlane.FullPathName;

            if (!System.IO.Directory.Exists(targetPath)) return null;

            string newPath = System.IO.Path.Combine(targetPath, foldername);
            //string sub = newPath.Substring(0);
            //string path = System.IO.Path.Combine(sub, "FinFileAttri");
            int i = 0;
            while ((System.IO.Directory.Exists(newPath)) && (i < 100))
            {
                i++;
                newPath = System.IO.Path.Combine(targetPath,
                    foldername + (i.ToString()));
                // CreateXmlFile(newPath);
            }
            System.IO.Directory.CreateDirectory(newPath);
            RefreshFolderPlanesAndSelectedNavTree(null);
            return newPath;
        }

        RelayCommand newFolderCommand;
        public ICommand NewFolderCommand
        {
            // test CanExecute not correct hack, to do?
            get { return newFolderCommand ?? (newFolderCommand = new RelayCommand(NewFolder, x => selectedFolderPlane.FullPathName != null)); }
        }
        //private NewWindow newWindow;
        public void NewFolder(object p)
        {
            NewWindow newWindow = new NewWindow();
            newWindow.ShowDialog();
            // Saw once error during heavy copying, not yet reproducable, new folder/copy to .exe folder 
            if ((SelectedFolderPlane == null) ||
                (SelectedFolderPlane.FullPathName == "") ||
                (SelectedPath == "")) return;
            if (newWindow.TxtChineseName.Text.Length != 0 && newWindow.TxtEnglishName.Text.Length != 0 && newWindow.IsEnter)
            {
                string targetPath = SelectedFolderPlane.FullPathName;

                if (!System.IO.Directory.Exists(targetPath)) return;

                string foldName = newWindow.TxtEnglishName.Text;// + '&' + newWindow.TxtChineseName.Text;
                string newPath = System.IO.Path.Combine(targetPath, foldName);
                string sub = newPath.Substring(0);
                string path = System.IO.Path.Combine(sub, "FinFileAttri");
                int i = 0;
                while ((System.IO.Directory.Exists(newPath)) && (i < 100))
                {
                    i++;
                    newPath = System.IO.Path.Combine(targetPath,
                        foldName + (i.ToString()));
                    // CreateXmlFile(newPath);
                }
                System.IO.Directory.CreateDirectory(newPath);

                CreateDirXmlFile(path + ".xml", newWindow.TxtChineseName.Text, newWindow.TxtEnglishName.Text);
            }

            RefreshFolderPlanesAndSelectedNavTree(null);
        }
        public void CopyDirectory(string sourceDirName, string destDirName)
        {

            try
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                    File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));

                }
                if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                    destDirName = destDirName + Path.DirectorySeparatorChar;
                var total = 0;
                string[] files = Directory.GetFiles(sourceDirName);
                foreach (string file in files)
                {
                    if (File.Exists(destDirName + Path.GetFileName(file)))
                        continue;
                    File.Copy(file, destDirName + Path.GetFileName(file), true);
                    File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
                    total++;
                }
                string[] dirs = Directory.GetDirectories(sourceDirName);
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, destDirName + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", true);
                sw.Write(ex.Message + "     " + DateTime.Now + "\r\n");
                sw.Close();
            }
        }
         //      从这里开始看
        /************************************************************
         ****=====================导入模型========
        ***************************************************************/
        RelayCommand _imPortModelCommand;
        public ICommand ImPortModelCommand
        {
            get { return _imPortModelCommand ?? (_imPortModelCommand = new RelayCommand(x => ImPortModel(x))); }
        }

        //家具FurnitureAttributes属性
        public FurnitureTypeAttributes FurType;
        public FurnVerticalFreedom FurFreedom;
        //   public FurnTransparent FurnTransparent;
       
        public int FurnTransparent; //透明度
        public double DefaultDisFromFloor;//默认离地高度

        //家具FinFileAttri
        public FinFileType FinType;
        public FinFileType LangItem;

        //  ======   导入模型  ====
        public void ImPortModel(object p)
        {
            ImportWindow importmodel = new ImportWindow();
            importmodel.ShowDialog();

            string namefolder = null;
            string str = importmodel.TxtSourcePath.Text;
            string[] s = str.Split(new char[] { '\\' });

            namefolder = s[s.Length - 1];

            int andCount = 0;
            for (int i = 0; i < namefolder.Length; i++)
            {
                if (namefolder[i] == '&')
                {
                    andCount++;
                }
            }
            if (andCount == 1)
            {
                string[] sname = namefolder.Split(new char[] { '&' });

                importmodel.TxtModelChineseName.Text = sname[1];
                importmodel.TxtModelEnglishName.Text = sname[0];
            }

            //保存家具xml
            if (importmodel.RbFurniture.IsChecked == true &&
                importmodel.TxtSourcePath.Text.Length != 0 &&
                importmodel.TxtModelChineseName.Text.Length != 0 &&
                importmodel.TxtModelEnglishName.Text.Length != 0 && importmodel.isEnter)
            {
                string dir = NewObjFolder(namefolder);
                string sourcepath = importmodel.TxtSourcePath.Text;//源路径
                string destpath = dir;//目标路径

                CopyDirectory(sourcepath, destpath);
                string subPath = destpath.Substring(0);
                string path = System.IO.Path.Combine(subPath, "FurnitureAttributes");
                string namepath = System.IO.Path.Combine(subPath, "FinFileAttri");

                //家具类型
                if (importmodel.RbInWall.IsChecked == true)
                {
                    FurType = FurnitureTypeAttributes.InWall;
                }
                else if (importmodel.RbOnWall.IsChecked == true)
                {
                    FurType = FurnitureTypeAttributes.OnWall;
                }
                //家具属性
                if (importmodel.RbInFloor.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.InFloor;
                }
                else if (importmodel.RbOnFloor.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.OnFloor;
                }
                else if (importmodel.RbFloorMove.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.Free;
                }
                else if (importmodel.RbOnCelling.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.OnCeiling;
                }
                else if (importmodel.RbInCelling.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.InCeiling;
                }
                //透明度
                if (importmodel.RbTransparent.IsChecked == true)
                {
                    FurnTransparent = 100;
                }
                else if (importmodel.RbTranslucent.IsChecked == true)
                {
                    FurnTransparent = 0;
                }
                //家具离地高度
                if (importmodel.TxtDefDisFromFloor.Text != "")
                {
                    DefaultDisFromFloor = double.Parse(importmodel.TxtDefDisFromFloor.Text);
                }
                if (importmodel.TxtDefDisFromFloor.Text == "")
                {
                    DefaultDisFromFloor = 0;
                }
                if (File.Exists("FurnitureAttributes.xml"))
                {
                    File.Delete("FurnitureAttributes.xml");
                }
                if (File.Exists("FinFileAttri.xml"))
                {
                    File.Delete("FinFileAttri.xml");
                }

                int isDirectory = 0;
                DirectoryInfo search = new DirectoryInfo(str);   //

                FileSystemInfo[] fsinfos = search.GetFileSystemInfos();  //获取目录sourthPath下所有文件
                foreach (FileSystemInfo fsinfo in fsinfos)
                {
                    if (fsinfo is DirectoryInfo) //判断是否为文件夹
                    {
                        isDirectory++;
                        int count = 0;                      
                        string subxmlPath = destpath+'\\'+fsinfo.ToString().Substring(0);
                        string furnAttribuesPath = System.IO.Path.Combine(subxmlPath,
                            "FurnitureAttributes");
                        string finFileAttriPath = System.IO.Path.Combine(subxmlPath,
                            "FinFileAttri");

                        string ss = fsinfo.ToString();
                        for (int i = 0; i < ss.Length; i++)
                        {
                            if (ss[i]=='&')
                            {
                                count++;
                            }                           
                        }
                        if (count==1)
                        {
                            //假如文件夹是以这种方式命名“家具&Furniture "
                            string[] foldername =
                            fsinfo.ToString().Split(new char[] { '&' });

                            if (importmodel.isEnter)
                            {
                                CreateFurnitureAttributes(furnAttribuesPath + ".xml");

                                CreateFinFileAttriXmlFiles(
                                    finFileAttriPath + ".xml", foldername[1],
                                    foldername[0]);

                                CreateDirXmlFile(namepath + ".xml",
                                    importmodel.TxtModelChineseName.Text,
                                    importmodel.TxtModelEnglishName.Text);
                            }
                        }
                        else
                        {
                            CreateFurnitureAttributes(furnAttribuesPath + ".xml");

                            //CreateFinFileAttriXmlFiles(
                            //    finFileAttriPath + ".xml", fsinfo.ToString(),
                            //   fsinfo.ToString());

                            CreateFinFileAttriXmlFiles(
                              finFileAttriPath + ".xml", importmodel.TxtModelChineseName.Text,
                             importmodel.TxtModelEnglishName.Text);

                            CreateDirXmlFile(namepath + ".xml",
                                importmodel.TxtModelChineseName.Text,
                                importmodel.TxtModelEnglishName.Text);
                        }


                    }
                    //else
                    //{
                    //    if (importmodel.isEnter)
                    //    {
                    //        CreatObjXmlFiles(path + ".xml", importmodel.TxtModelChineseName.Text,
                    //       importmodel.TxtModelEnglishName.Text);
                    //        CreateFinFileAttriXmlFiles(namepath + ".xml", importmodel.TxtModelChineseName.Text,
                    //           importmodel.TxtModelEnglishName.Text);

                    //        //CreateDirXmlFile(namepath + ".xml",
                    //        //    importmodel.TxtModelChineseName.Text,
                    //        //    importmodel.TxtModelEnglishName.Text);
                    //    }                      
                    //}
                }
                if (isDirectory==0)//导入单个模型文件夹
                {
                    CreateFurnitureAttributes(path + ".xml");

                    CreateFinFileAttriXmlFiles(
                      namepath + ".xml", importmodel.TxtModelChineseName.Text,
                     importmodel.TxtModelEnglishName.Text);
                }
               
            }
            //生成材质xml
            else if (importmodel.RbMaterial.IsChecked ==
                     true && importmodel.TxtSourcePath.Text.Length != 0 &&
                     importmodel.TxtModelChineseName.Text.Length != 0 &&
                     importmodel.TxtModelEnglishName.Text.Length != 0 && importmodel.isEnter)
            {
                string dir = NewObjFolder(namefolder);
                string sourcepath = importmodel.TxtSourcePath.Text;
                string destpath = dir;
                CopyDirectory(sourcepath, destpath);
                string subPath = destpath.Substring(0);
                string path = System.IO.Path.Combine(subPath, "FinFileAttri");
                createMaterialXmlFiles(path + ".xml", importmodel.TxtModelChineseName.Text,
                   importmodel.TxtModelEnglishName.Text);
            }
            RefreshFolderPlanesAndSelectedNavTree(null);
        }
        /// <summary>
        /// 生成材质 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="chineseName"></param>
        /// <param name="englishName"></param>
        private void createMaterialXmlFiles(string path, string chineseName, string englishName)
        {
            FinFileAttributes finFile = new FinFileAttributes
            {
                FileType = FinFileType.Material
            };
            LangItem lang = new LangItem
            {
                Content = chineseName,
                Lang = "zh-CN"
            };
            LangItem lang2 = new LangItem
            {
                Content = englishName,
                Lang = "en-US"
            };
            finFile.Name = new LangItem[2];
            finFile.Name[0] = lang;
            finFile.Name[1] = lang2;
            XmlSerializer.SaveToXml(path, finFile, finFile.GetType(), "FinFileAttributes");
        }
      
        public void CreatObjXmlFiles(string path, string chineseName, string englishName)
        {
            FurnitureAttributes furAttributes = new FurnitureAttributes();

            FinFileAttributes finfile = new FinFileAttributes();
            LangItem lang = new LangItem
            {
                Content = chineseName,
                Lang = "zh-CN"
            };
            LangItem lang2 = new LangItem
            {
                Content = englishName,
                Lang = "en-US"
            };
            finfile.Name = new LangItem[2];
            finfile.Name[0] = lang;
            finfile.Name[1] = lang2;

            furAttributes.Type = FurType;
            furAttributes.Freedom = FurFreedom;
            furAttributes.FurnTransparent = FurnTransparent;
            furAttributes.DefaultDisFromFloor = DefaultDisFromFloor;

            XmlSerializer.SaveToXml(path, furAttributes, furAttributes.GetType(), "FurnitureAttributes");

            // XmlSerializer.SaveToXml(path, finfile, finfile.GetType(), "FinFileAttributes");
        }
        /// <summary>
        /// 生成家具 FurnitureAttributes.xml 文件
        /// </summary>
        /// <param name="path"></param>
        public void CreateFurnitureAttributes(string path)
        {
            FurnitureAttributes furAttributes = new FurnitureAttributes();
            furAttributes.Type = FurType;
            furAttributes.Freedom = FurFreedom;
            furAttributes.FurnTransparent = FurnTransparent;
            furAttributes.DefaultDisFromFloor = DefaultDisFromFloor;

            XmlSerializer.SaveToXml(path, furAttributes, furAttributes.GetType(), "FurnitureAttributes");
        }
        /// <summary>
        /// 生成 FinFileAttri.xml 文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="chineseName"></param>
        /// <param name="englishName"></param>
        private void CreateFinFileAttriXmlFiles(string path, string chineseName, string englishName)
        {
            FinFileAttributes finFile = new FinFileAttributes
            {
                FileType = FinFileType.Obj
            };
            LangItem lang = new LangItem
            {
                Content = chineseName,
                Lang = "zh-CN"
            };
            LangItem lang2 = new LangItem
            {
                Content = englishName,
                Lang = "en-US"
            };
            finFile.Name = new LangItem[2];
            finFile.Name[0] = lang;
            finFile.Name[1] = lang2;
            XmlSerializer.SaveToXml(path, finFile, finFile.GetType(), "FinFileAttributes");
        }

        /// <summary>
        /// 生成路径的xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="chineseName"></param>
        /// <param name="englishName"></param>
        public void CreateDirXmlFile(string path, string chineseName, string englishName)
        {
            FinFileAttributes finFile = new FinFileAttributes
            {
                FileType = FinFileType.Dir
            };

            LangItem lang = new LangItem
            {
                Content = chineseName,
                Lang = "zh-CN"
            };

            LangItem lang2 = new LangItem
            {
                Content = englishName,
                Lang = "en-US"
            };

            finFile.Name = new LangItem[2];
            finFile.Name[0] = lang;
            finFile.Name[1] = lang2;

            XmlSerializer.SaveToXml(path, finFile, finFile.GetType(), "FinFileAttributes");
        }

       
        private RelayCommand editModel;

        public ICommand EditModel
        {
            get
            {
                return editModel ??
                       (editModel = new RelayCommand(x => EditingModel(x), x => SelectedFolderItems.Count != 0 && CheckDir()));
            }
        }

        private bool CheckDir()
        {          
            int selectCount = 0;
            for (int i = 0; i < SelectedFolderItems.Count; i++)
            {
                string sourthpath = SelectedFolderItems[i];
                if (System.IO.Directory.Exists(sourthpath))
                {
                    selectCount++;
                }
            }
            if (selectCount== SelectedFolderItems.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private EditWindow1 _editWindow1;
        /// <summary>
        /// 编辑模型
        /// </summary>
        /// <param name="p"></param>
        public void EditingModel(object p)
        {
            if (selectedFolderItems == null) return;
            if (selectedFolderItems.Count == 0) return;
            string str = null;

            // to do: test if folder can be deleted, send notification if not
            for (int i = 0; i <= SelectedFolderItems.Count - 1; i++)
            {
                str = SelectedFolderItems[i];
            }

            DirectoryInfo TheFolder =
                     new DirectoryInfo(str);

            //string destdir = str.Substring(0);
            FileInfo furFileInfo = new FileInfo(str + "\\" + "FurnitureAttributes.xml");
            FileInfo finFileInfo =
                new FileInfo(str + "\\" + "FinFileAttri.xml");

            if (furFileInfo.Exists && finFileInfo.Exists) //如果是家具模型
            {
                _editWindow1 = new EditWindow1();
                //家具类型
                _editWindow1.RbFurniture.IsChecked = true;
                try
                {
                    var furAttributes =
                       XmlSerializer.LoadFromXml<FurnitureAttributes>(str + '\\' +
                                                   "FurnitureAttributes.xml");  //反序列化

                    var finfile =
                        XmlSerializer.LoadFromXml<FinFileAttributes>(str + '\\' +
                                                                     "FinFileAttri.xml");  //反序列化
                    //EditWindow1 editWindow1=new EditWindow1();
                    _editWindow1.TxtSourcePath.Text = str;
                    _editWindow1.TxtModelChineseName.Text =
                        finfile.Name[0].Content.Substring(0);
                    _editWindow1.TxtModelEnglishName.Text =
                        finfile.Name[1].Content.Substring(0);

                    _editWindow1.TxtSourcePath.IsEnabled = false;

                    _editWindow1.RbMaterial.IsEnabled = false;

                    if (furAttributes.Type == FurnitureTypeAttributes.InWall)
                    {
                        _editWindow1.RbInWall.IsChecked = true;
                    }
                    else if (furAttributes.Type == FurnitureTypeAttributes.OnWall)
                    {
                        _editWindow1.RbOnWall.IsChecked = true;
                    }
                    if (furAttributes.Freedom == FurnVerticalFreedom.InFloor)
                    {
                        _editWindow1.RbInFloor.IsChecked = true;
                    }
                    else if (furAttributes.Freedom == FurnVerticalFreedom.OnFloor)
                    {
                        _editWindow1.RbOnFloor.IsChecked = true;
                    }
                    else if (furAttributes.Freedom == FurnVerticalFreedom.Free)
                    {
                        _editWindow1.RbFloorMove.IsChecked = true;
                    }
                    else if (furAttributes.Freedom == FurnVerticalFreedom.OnCeiling)
                    {
                        _editWindow1.RbOnCelling.IsChecked = true;
                    }
                    else if (furAttributes.Freedom == FurnVerticalFreedom.InCeiling)
                    {
                        _editWindow1.RbInCelling.IsChecked = true;
                    }

                    if (furAttributes.FurnTransparent == 100)
                    {
                        _editWindow1.RbTransparent.IsChecked = true;
                    }
                    else if (furAttributes.FurnTransparent == 0)
                    {
                        _editWindow1.RbTranslucent.IsChecked = true;
                    }                
                    _editWindow1.TxtDefaultFromFloor.Text =furAttributes.DefaultDisFromFloor.ToString(CultureInfo.InvariantCulture);
                    _editWindow1.ShowDialog();
                    if (_editWindow1.RbFloorMove.IsChecked == true)
                    {
                       
                    }
                    else
                    {
                        DefaultDisFromFloor = 0;
                    }
                    if (_editWindow1.IsEnter)
                    {
                        SetObjXmlFiles(); //重新设置xml文件
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("您编辑的模型文件里的XML文件格式不对,请检查您的文件格式:\n" + ex);

                    return;
                }
            }

            else if (furFileInfo.Exists == false && finFileInfo.Exists)
            {
                try
                {
                    var finfile =
                   XmlSerializer.LoadFromXml<FinFileAttributes>(str + '\\' +
                                                                "FinFileAttri.xml");
                    //EditWindow1 editWindow1=new EditWindow1();
                    if (finfile.FileType == FinFileType.Material)
                    {
                        _editWindow1 = new EditWindow1();

                        _editWindow1.RbMaterial.IsChecked = true;
                        _editWindow1.TxtSourcePath.IsEnabled = false;

                        _editWindow1.RbFurniture.IsEnabled = false;

                        _editWindow1.TxtSourcePath.Text = str;
                        _editWindow1.TxtModelChineseName.Text =
                            finfile.Name[0].Content.Substring(0);
                        _editWindow1.TxtModelEnglishName.Text =
                            finfile.Name[1].Content.Substring(0);
                        _editWindow1.GdProperty.Visibility = Visibility.Hidden;
                        _editWindow1.ShowDialog();

                        if (_editWindow1.IsEnter)
                        {
                            SetMaterialXmlFiles();
                            //ReNameDirFolder(editWindow1.TxtModelEnglishName.Text);
                        }
                    }
                    if (finfile.FileType == FinFileType.Dir)
                    {
                        EditDir editDir = new EditDir();
                        editDir.TxtChineseName.Text =
                            finfile.Name[0].Content.Substring(0);
                        editDir.TxtEnglishName.Text =
                           finfile.Name[1].Content.Substring(0);
                        editDir.ShowDialog();
                        if (editDir.IsEnter)
                        {
                            string sub = str.Substring(0);
                            string path = System.IO.Path.Combine(sub, "FinFileAttri");
                            CreateDirXmlFile(path + ".xml", editDir.TxtChineseName.Text, editDir.TxtEnglishName.Text);
                            ReNameDirFolder(editDir.TxtEnglishName.Text);
                        }
                    }
                    if (finfile.FileType == FinFileType.Obj)
                    {
                        //MessageBox.Show("没有您要编辑的模型或者材质，请确保您选择的路径是正确的");

                        _editWindow1 = new EditWindow1();
                        _editWindow1.TxtSourcePath.Text = str;
                        _editWindow1.TxtModelChineseName.Text =
                            finfile.Name[0].Content.Substring(0);
                        _editWindow1.TxtModelEnglishName.Text =
                            finfile.Name[1].Content.Substring(0);

                        _editWindow1.TxtSourcePath.IsEnabled = false;

                        _editWindow1.RbMaterial.IsEnabled = true;
                        _editWindow1.ShowDialog();
                        if (_editWindow1.IsEnter)
                        {
                            SetObjXmlFiles();
                            // ReNameDirFolder(editWindow1.TxtModelEnglishName.Text);
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("您编辑的模型文件里的XML文件格式不对,请检查您的文件格式:\n" + ex);
                    return;
                }
            }

            else if (furFileInfo.Exists && finFileInfo.Exists == false)
            {

                _editWindow1 = new EditWindow1();
                int count = 0;

                // File.Delete(furFileInfo.ToString());

                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    if (
                        NextFile.Name.Substring(
                            NextFile.Name.LastIndexOf(".")) == ".obj")
                    {
                        //xml文件，其实的也类型。也可以改成lamda表达式。
                        count++;
                    }
                }

                if (count != 0)
                {
                    //家具类型
                    _editWindow1.RbFurniture.IsChecked = true;
                    File.Delete(furFileInfo.ToString());

                    _editWindow1.TxtSourcePath.Text = str;

                    _editWindow1.ShowDialog();
                    if (_editWindow1.IsEnter)
                    {
                        SetObjXmlFiles();
                    }
                }
                else
                {
                    MessageBox.Show("没有您要编辑的模型或者材质，请确保您选择的路径是正确的");
                }
            }
            else if (furFileInfo.Exists == false && finFileInfo.Exists == false)
            {
                //  MessageBox.Show("没有您要编辑的模型或者材质，请确保您选择的路径是正确的");
                _editWindow1 = new EditWindow1();
                int count = 0;

                _editWindow1.TxtSourcePath.Text = str;

                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    if (
                        NextFile.Name.Substring(
                            NextFile.Name.LastIndexOf(".")) == ".obj")
                    {
                        //xml文件，其实的也类型。也可以改成lamda表达式。
                        count++;
                    }
                }

                if (count != 0)
                {
                    _editWindow1.ShowDialog();
                    if (_editWindow1.IsEnter)
                    {
                        if (_editWindow1.RbFurniture.IsChecked == true)
                        {
                            SetObjXmlFiles();
                        }
                        if (_editWindow1.RbMaterial.IsChecked == true)
                        {
                            SetMaterialXmlFiles();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("没有您要编辑的模型或者材质，请确保您选择的路径是正确的");
                }
            }
            RefreshFolderPlanesAndSelectedNavTree(null);
        }
        /// <summary>
        /// 编辑模型时重新生成的xml文件
        /// </summary>
        private void SetObjXmlFiles()
        {
            //保存家具xml
            if (_editWindow1.RbFurniture.IsChecked == true &&
                _editWindow1.TxtSourcePath.Text.Length != 0)
            {
                //string sourcepath = _editWindow1.TxtSourcePath.Text;
                //string destpath = SelectedFolderPlane.FullPathName;
                // CopyDirectory(sourcepath, destpath);
                //string destpath = dir;
                string str = null;
                for (int i = 0; i <= SelectedFolderItems.Count - 1; i++)
                {
                    str = SelectedFolderItems[i];
                }
                //CopyDirectory(sourcepath, destpath);
                string subPath = str.Substring(0);
                string path = System.IO.Path.Combine(subPath, "FurnitureAttributes");
                string namepath = System.IO.Path.Combine(subPath, "FinFileAttri");

                //家具类型
                if (_editWindow1.RbInWall.IsChecked == true)
                {
                    FurType = FurnitureTypeAttributes.InWall;
                }
                else if (_editWindow1.RbOnWall.IsChecked == true)
                {
                    FurType = FurnitureTypeAttributes.OnWall;
                }
                //家具属性
                if (_editWindow1.RbInFloor.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.InFloor;
                }
                else if (_editWindow1.RbOnFloor.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.OnFloor;
                }
                else if (_editWindow1.RbFloorMove.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.Free;
                }
                else if (_editWindow1.RbOnCelling.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.OnCeiling;
                }
                else if (_editWindow1.RbInCelling.IsChecked == true)
                {
                    FurFreedom = FurnVerticalFreedom.InCeiling;
                }
                //透明度
                if (_editWindow1.RbTransparent.IsChecked == true)
                {
                    FurnTransparent = 100;
                }
                else if (_editWindow1.RbTranslucent.IsChecked == true)
                {
                    FurnTransparent = 0;
                }                
                if (_editWindow1.RbFloorMove.IsChecked == true)
                {
                    if (_editWindow1.TxtDefaultFromFloor.Text != "")
                    {
                        DefaultDisFromFloor =
                        double.Parse(_editWindow1.TxtDefaultFromFloor.Text);
                    }
                }
                else
                {
                    DefaultDisFromFloor = 0;
                }
                if (File.Exists("FurnitureAttributes.xml"))
                {
                    File.Delete("FurnitureAttributes.xml");
                }
                if (File.Exists("FinFileAttri.xml"))
                {
                    File.Delete("FinFileAttri.xml");
                }
                //CreatObjXmlFiles(path + ".xml", _editWindow1.TxtModelChineseName.Text,
                //    _editWindow1.TxtModelEnglishName.Text);

                CreateFurnitureAttributes(path + ".xml");
                CreateFinFileAttriXmlFiles(namepath + ".xml", _editWindow1.TxtModelChineseName.Text,
                  _editWindow1.TxtModelEnglishName.Text);
            }
        }
       /// <summary>
       /// 编辑材质文件，设置xml文件
       /// </summary>
        private void SetMaterialXmlFiles()
        {
            string sourcepath = _editWindow1.TxtSourcePath.Text;
            //string destpath = SelectedFolderPlane.FullPathName;
            //string subPath = sourcepath.Substring(0);

            string path = System.IO.Path.Combine(sourcepath, "FinFileAttri");
            createMaterialXmlFiles(path + ".xml", _editWindow1.TxtModelChineseName.Text,
               _editWindow1.TxtModelEnglishName.Text);
        }


        /*****************批量导入xml文件******************************
         *************************************************************/
        RelayCommand numbersImportCommand;
        public ICommand NumbersImportCommand
        {
            get
            {
                return numbersImportCommand ??
                       (numbersImportCommand =
                           new RelayCommand(x => NumbersImportXml(x),
                               x => SelectedFolderItems.Count == 1 && CheckSubDir()));
            }          
        }

        private bool CheckSubDir()
        {
            int dirCount = 0;
            if (SelectedFolderItems.Count == 1)
            {
                string sourthPath = SelectedFolderItems[0];
                DirectoryInfo search = new DirectoryInfo(sourthPath);   //
                if (System.IO.Directory.Exists(sourthPath))
                {
                    FileSystemInfo[] fsinfos = search.GetFileSystemInfos();  //获取目录sourthPath下所有文件
                    foreach (FileSystemInfo fsinfo in fsinfos)
                    {
                        if (fsinfo is DirectoryInfo)
                        {
                            dirCount++;
                        }
                    }
                }             
            }
            if (dirCount!=0)
            {
                return true;
            }
            else
            {
                return false;
            }        
        }
        //批量生成xml文件
        public void NumbersImportXml(object p)
        {
            EditWindow1 edwindow = new EditWindow1();

            if (SelectedFolderItems == null)
             return;
            if (SelectedFolderItems.Count == 0)
            {
                MessageBox.Show("您的路径不对,请选择正确的目录文件路径，再生成XML");
                return;
            }              
            edwindow.TxtSourcePath.Text = SelectedFolderItems[0];

            string sourthPath = SelectedFolderItems[0];
            DirectoryInfo search = new DirectoryInfo(sourthPath);   //

            FileSystemInfo[] fsinfos = search.GetFileSystemInfos();  //获取目录sourthPath下所有文件
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo.Name== "FinFileAttri.xml")
                {
                    var finfile =
                       XmlSerializer.LoadFromXml<FinFileAttributes>(sourthPath + '\\' +
                                                                    "FinFileAttri.xml");
                    if (finfile.FileType==FinFileType.Dir)
                    {
                        edwindow.TxtModelChineseName.Text =
                        finfile.Name[0].Content.Substring(0);
                        edwindow.TxtModelEnglishName.Text =
                            finfile.Name[1].Content.Substring(0);
                    }
                }
            }
        

            edwindow.ShowDialog();
            //家具类型
            if (edwindow.RbInWall.IsChecked == true)
            {
                FurType = FurnitureTypeAttributes.InWall;
            }
            else if (edwindow.RbOnWall.IsChecked == true)
            {
                FurType = FurnitureTypeAttributes.OnWall;
            }
            //家具属性
            if (edwindow.RbInFloor.IsChecked == true)
            {
                FurFreedom = FurnVerticalFreedom.InFloor;
            }
            else if (edwindow.RbOnFloor.IsChecked == true)
            {
                FurFreedom = FurnVerticalFreedom.OnFloor;
            }
            else if (edwindow.RbFloorMove.IsChecked == true)
            {
                FurFreedom = FurnVerticalFreedom.Free;
            }
            else if (edwindow.RbOnCelling.IsChecked == true)
            {
                FurFreedom = FurnVerticalFreedom.OnCeiling;
            }
            else if (edwindow.RbInCelling.IsChecked == true)
            {
                FurFreedom = FurnVerticalFreedom.InCeiling;
            }
            //透明度
            if (edwindow.RbTransparent.IsChecked == true)
            {
                FurnTransparent = 100;
            }
            else if (edwindow.RbTranslucent.IsChecked == true)
            {
                FurnTransparent = 0;
            }
            if (edwindow.TxtDefaultFromFloor.Text != "")
            {
                DefaultDisFromFloor = double.Parse(edwindow.TxtDefaultFromFloor.Text);
            }
           
            //if (File.Exists("FurnitureAttributes.xml"))
            //{
            //    File.Delete("FurnitureAttributes.xml");
            //}
            //if (File.Exists("FinFileAttri.xml"))
            //{
            //    File.Delete("FinFileAttri.xml");
            //}
           
         
            foreach (FileSystemInfo fsinfo in fsinfos)
            {
                if (fsinfo is DirectoryInfo)     //判断是否为文件夹
                {
                    int count = 0;
                    DirectoryInfo TheFolder = new DirectoryInfo(fsinfo.FullName);
                    foreach (FileInfo NextFile in TheFolder.GetFiles())
                    {
                        if (NextFile.Name.Substring(NextFile.Name.LastIndexOf(".")) == ".obj")
                        {
                            //xml文件，其实的也类型。也可以改成lamda表达式。
                            count++;
                        }
                    }
                    //当目录下有 后缀名为 obj 时 才在路径下生成 FurnitureAttributes.xml文件 和 FinFileAttri.xml 文件
                    if (count > 0)
                    {
                        string subPath = TheFolder.ToString().Substring(0);
                        string path = System.IO.Path.Combine(subPath, "FurnitureAttributes");
                        //string namepath = System.IO.Path.Combine(subPath, "FinFileAttri");

                        string namepath = System.IO.Path.Combine(subPath, "FinFileAttri");
                        if (edwindow.IsEnter)
                        {
                            CreateFurnitureAttributes(path + ".xml");
                            CreateFinFileAttriXmlFiles(namepath + ".xml", edwindow.TxtModelChineseName.Text, edwindow.TxtModelEnglishName.Text);
                        }

                    }
                    else
                    {
                        //CreateFinFileAttriXmlFiles();
                        string subPath = TheFolder.ToString().Substring(0);
                        //string namepath = System.IO.Path.Combine(subPath, "FinFileAttri");
                        //CreateFinFileAttriXmlFiles(namepath + ".xml", edwindow.TxtModelChineseName.Text, edwindow.TxtModelEnglishName.Text);
                    }
                }

                if (!File.Exists(sourthPath+'\\'+"FinFileAttri.xml"))
                {
                    string namefolder = null;
                    //string str = importmodel.TxtSourcePath.Text;
                    string chineseName = null;
                    string englishName = null;

                    string[] s = sourthPath.Split(new char[] { '\\' });

                    namefolder = s[s.Length - 1];

                    int andCount = 0;
                    for (int i = 0; i < namefolder.Length; i++)
                    {
                        if (namefolder[i] == '&')
                        {
                            andCount++;
                        }
                    }
                    if (andCount == 1)
                    {
                        //当批量导入的目录下面 有文件夹的名字是以“家具&Furnitur” 这类型格式时，
                        //该文件夹下的FinFileAttri.xml文件下的中文名则为：家具，英文名则为：furni
                        string[] sname = namefolder.Split(new char[] { '&' });
                        chineseName = sname[1];
                        englishName = sname[0];
                    }
                    else //否则该文件夹下的FinFileAttri.xml文件下的中文名与英文名都为该文件夹的名字
                    {
                        chineseName = namefolder;
                        englishName = namefolder;
                    }
                    string sub = sourthPath.Substring(0);
                    string path = System.IO.Path.Combine(sub, "FinFileAttri");
                    CreateDirXmlFile(path +".xml", chineseName, englishName);
                }             
            }           
        }
        /*==============================================================================================*/
    }
}
