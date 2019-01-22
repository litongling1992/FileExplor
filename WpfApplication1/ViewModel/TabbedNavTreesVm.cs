using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM;
using System.Collections.ObjectModel;
using WpfApplication1.Model;

namespace WpfApplication1.ViewModel
{
    public class TabbedNavTreesVm : ViewModelBase
    {
        public List<string> listNamesNavTrees { get; set; }  // ... is set once

        // For the tabbed NavTrees, set on construction
        private ObservableCollection<NavTreeVm> navTrees;
        public ObservableCollection<NavTreeVm> NavTrees
        {
            get { return navTrees ?? (navTrees = new ObservableCollection<NavTreeVm>()); }
        }

        // SelectedNavTree
        private NavTreeVm selectedNavTree;
        public NavTreeVm SelectedNavTree
        {
            get { return selectedNavTree; }
            set
            {
                SetProperty(ref selectedNavTree, value, "SelectedNavTree");
                SelectedNavTree.RebuildTree();
            }
        }

        // In view we use uniform grid for tabs; without specification: auto generation row*colomn => n*n; so extra rows included
        // By using MaxRowsNavTrees we set the max. nr of rows in the Tabs of the NavTrees
        private int maxRowsNavTrees;
        public int MaxRowsNavTrees
        {
            get { return maxRowsNavTrees; }
            set { SetProperty(ref maxRowsNavTrees, value, "MaxRowsNavTrees"); }
        }

        // We compute later nrTrees/MaxRowsNavTrees using TabsPerRow (= using some knowledge of view)
        // We could avoid this by using a converter from NrTrees to MaxRowsNavTrees in the view but that results in more code..
        private const int TabsPerRow = 3;

        // Constructor, determines what trees are placed in Tabs
        public TabbedNavTreesVm()
        {
            // Generate trees, here once and fixed
            navTrees = new ObservableCollection<NavTreeVm>();

            listNamesNavTrees = NavTreeRootItemUtils.ListNavTreeRootItemsByConvention();
            NavTreeVm newTree;

            MaxRowsNavTrees = 2;
            int nrTrees = MaxRowsNavTrees * TabsPerRow;

            // Here NavTrees are created. A number of trees is generated using constructor NavTree()
            // 
            int nrRootItems = listNamesNavTrees.Count();
            for (int rootNr = 0; rootNr < nrTrees; rootNr++)
            {
                // just shortcut to create a number of Trees
                newTree = new NavTreeVm(rootNr, false);
                newTree.TreeName = (rootNr < nrRootItems) ? listNamesNavTrees[rootNr] : listNamesNavTrees[0] + (rootNr - nrRootItems).ToString("d");
                NavTrees.Add(newTree);
            }
        }
    }
}
