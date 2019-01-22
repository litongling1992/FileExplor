using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
// Add reference, Com tab, choose Microsoft Shell Controls and Automation

namespace WpfApplication1.Model
{
    // FolderPlane and FolderPlaneItems for the FolderPlane view

    // Define Interfaces and Classes for FolderPlane and FolderItem
    // We use full pathname as independant interface
    // Note that we translate System.IO to our types

    // To do: better design for exceptional situations, test more, do some visual testing using Windows File Explorer

    public interface IFolderPlane
    {
        string FullPathName { get; set; }

        // For display (in TabItem)
        string FriendlyName { get; set; }

        void SetFolderPlane(string path, bool clear = false);
        void RefreshFolderPlane();

        //constructor
        //FolderPlane (string path);

        // Items displayed in FolderMap
        ObservableCollection<FolderPlaneItem> FolderPlaneItems { get; set; }
    }

    public class FolderPlaneItem
    {
        public String FullPathName { get; set; }

        public String Name { get; set; }
        public String Ext { get; set; }
        public String Date { get; set; } // or String <-> DateTime
        public long Size { get; set; }
        public BitmapSource MyIcon { get; set; }
    }

    public class FolderPlane : IFolderPlane
    {
        public string FullPathName { get; set; }
        public string FriendlyName { get; set; }

        // Items displayed in FolderMap
        public ObservableCollection<FolderPlaneItem> FolderPlaneItems { get; set; }

        // A constructor
        public FolderPlane()
        {
            FolderPlaneItems = new ObservableCollection<FolderPlaneItem> { };
        }

        // Another constructor using the previous A constructor
        public FolderPlane(string path) : this()
        {
            SetFolderPlane(path);
        }

        public void SetFolderPlane(string path, bool clear = false)
        {
            if (!Directory.Exists(path)) path = "";
            FullPathName = path;
           
            // If not valid path Clear items and return 
            if ((path == null) || (path == "")) 
            {
                FolderPlaneItems.Clear();
                return; 
            }

            if (clear) FolderPlaneItems.Clear();

            bool isDrive = FolderPlaneUtils.IsDrive(path);

            if (isDrive)
            {
                FriendlyName = path;

                DriveInfo drive = new DriveInfo(path);
                DirectoryInfo di = new DirectoryInfo(((DriveInfo)drive).RootDirectory.Name);

                GetFoldersAndFiles(di);
                return;
            }

            bool isFolder = FolderPlaneUtils.IsFolder(path);

            if (isFolder)
            {
                // Get friendlyname, last foldername 
                string[] folders = path.Split('\\');
                string str = folders[folders.Length - 1];
                FriendlyName = FolderPlaneUtils.MyShortFriendlyName(str);

                DirectoryInfo di = new DirectoryInfo(path);

                GetFoldersAndFiles(di);
                return;
            }
        }

        public void RefreshFolderPlane()
        {
            string path = FullPathName;
            bool clear = true;
            SetFolderPlane(path, clear);
        }

        private void GetFoldersAndFiles(DirectoryInfo di)
        {
            FolderPlaneItem item;

            // List folders
            // optionally test on di.Attributes.HasFlag(FileAttributes.Hidden) and .HasFlag(FileAttributes.System))

            // to do: check if exception can stop other items from beiing processed
            try
            {
                DateTime dt;
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    item = new FolderPlaneItem();
                    item.FullPathName = dir.FullName;

                    item.Name = dir.Name;
                    item.Ext = "";

                    // We choose a fixed format 2011/01/01 hr min for date, conversion here
                    dt = dir.LastWriteTime; //DateTime.Now;
                    string format = " yyyy/MM/dd  HH.mm";
                    item.Date = dt.ToString(format);

                    item.Size = 0;
                    item.MyIcon = Utils.ImageCache.GetImage(dir.FullName);
                    FolderPlaneItems.Add(item);
                }


                // list files
                char[] aPoint = { '.' };
                foreach (FileInfo file in di.GetFiles())
                {
                    // Decision: fixed, don't show hidden files
                    if ((!file.Attributes.HasFlag(FileAttributes.Hidden)))
                    {
                        item = new FolderPlaneItem();
                        item.FullPathName = file.FullName;

                        item.Name = file.Name;
                        item.Ext = file.Extension.TrimStart(aPoint);

                        dt = file.LastWriteTime;
                        string format = " yyyy/MM/dd  HH.mm";
                        item.Date = dt.ToString(format);

                        item.Size = file.Length / 1024;
                        item.MyIcon = Utils.ImageCache.GetImage(file.FullName);
                        FolderPlaneItems.Add(item);
                    }
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

        }
    }
}
