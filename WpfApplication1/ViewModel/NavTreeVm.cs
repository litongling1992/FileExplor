using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication1.Model;
using System.Collections.ObjectModel;
using MVVM;
using System.Windows.Input;

namespace WpfApplication1.ViewModel
{

    public class NavTreeVm : ViewModelBase
    {

        // public ICommand SelectedPathFromTreeCommand moved to ViewModel
        
        // a Name to bind to the NavTreeTabs
        private string treeName = "";
        public string TreeName
        {
            get { return treeName; }
            set { SetProperty(ref treeName, value, "TreeName"); }
        }

        // RootNr determines nr of RootItem that is used as RootNode 
        private int rootNr;
        public int RootNr
        {
            get { return rootNr; }
            set { SetProperty(ref rootNr, value, "RootNr"); }
        }

        // RootChildren are used to bind to TreeView
        private ObservableCollection<INavTreeItem> rootChildren = new ObservableCollection<INavTreeItem> { };
        public ObservableCollection<INavTreeItem> RootChildren
        {
            get { return rootChildren; }
            set { SetProperty(ref rootChildren, value, "RootChildren"); }
        }

        public void RebuildTree(int pRootNr = -1, bool pIncludeFileChildren = false)
        {
            // First take snapshot of current expanded items
            List<String> SnapShot = NavTreeUtils.TakeSnapshot(rootChildren);

            // As a matter of fact we delete and construct the tree//RoorChildren again.....
            // Delete all rootChildren
            foreach (INavTreeItem item in rootChildren) item.DeleteChildren();
            rootChildren.Clear();

            // Create treeRootItem 
            if (pRootNr != -1) RootNr = pRootNr;
            NavTreeItem treeRootItem = NavTreeRootItemUtils.ReturnRootItem(RootNr, pIncludeFileChildren);
            if (pRootNr != -1) TreeName = treeRootItem.FriendlyName;

            // Copy children treeRootItem to RootChildren, set up new tree 
            foreach (INavTreeItem item in treeRootItem.Children) { RootChildren.Add(item); }

            //Expand previous snapshot
            NavTreeUtils.ExpandSnapShotItems(SnapShot, treeRootItem);
        }

        // Constructors
        public NavTreeVm(int pRootNumber = 0, bool pIncludeFileChildren = false)
        {
            // create a new RootItem given rootNumber using convention
            RootNr = pRootNumber;
            NavTreeItem treeRootItem = NavTreeRootItemUtils.ReturnRootItem(pRootNumber, pIncludeFileChildren);
            TreeName = treeRootItem.FriendlyName;

            // Delete RootChildren and init RootChildren ussing treeRootItem.Children
            foreach (INavTreeItem item in RootChildren) { item.DeleteChildren(); }
            RootChildren.Clear();

            foreach (INavTreeItem item in treeRootItem.Children) { RootChildren.Add(item); }
        }

        // Well I suppose with the implicit values these are just for the record/illustration  
        public NavTreeVm(int rootNumber) : this(rootNumber, false) { }
        public NavTreeVm() : this(0) { }
    }
}



