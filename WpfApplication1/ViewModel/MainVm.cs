using System.Windows.Data;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using System.Collections.Specialized;
using WpfApplication1.Model;
using Utils;
using System.Windows.Input;
using System.IO;
using MVVM;


namespace WpfApplication1.ViewModel
{

    // Note: for now we put all the commands and supporting procedures of partial class MainVM in file ViewModelCommands 

    // To do: 
    // - Choose MVVM framework and split this file in smaller parts
    //   TabbedNavTrees, FolderPlane+Tabs, SavedFolderTabs, FileManagement buttons

    public partial class MainVm : ViewModelBase
    {
        public TabbedNavTreesVm TabbedNavTrees { get; set;}

        // SelectedPath basis/entrypoint of all changes
        // Set in command SelectedPathFromTreeCommand, Set SelectedFolderPlane, OnFolderPlaneItemDoubleClick etc
        // Design just by building application. Became in time a little more complicated
        // To do: better analyses and design for invalid path etc
        private string selectedPath;
        public string SelectedPath
        {
            get
            {
                return selectedPath;
            }
            set
            {
                // Resolve shortcut, test if valid drive or folder.
                // If not valid, keep old value selectedPath

                // Handle shortcuts to folders. 
                string testValue = FolderPlaneUtils.ResolveIfShortCut(value);

                
                // "" considered a valid value, SetProperty 
                if (testValue == "") { SetProperty(ref selectedPath, testValue, "SelectedPath"); return; } 

                // Return on not existing testValue, otherwise SetProperty 
                if (!FolderPlaneUtils.hasWriteAccessToFolder(testValue)) return;
                if (!Directory.Exists(testValue)) return;

                if (!SetProperty(ref selectedPath, testValue, "SelectedPath")) return;
                

                // Now adapt FolderPlanes and SelectedFolderPlane for valid values not ""
                // Choices/Design when to create a new FolderPlane, use an existing or replace Current Folder
                // Can be made dependant shift or control keys

                // to do:
                // - Update history here. Total, per FolderPlane
                // - If we should include files we should handle/launch them here, no adaption of SelectedPath


                // Design Choice: If existing in FolderPlanes: set SelectedFolderPlane to that and done
                int indexInPlanes = GetIndexFolderPlanes(selectedPath);
                if (indexInPlanes != -1)
                {
                    //Position: to end or use existing place
                    SelectedFolderPlane = FolderPlanes[indexInPlanes];
                    UseCurrentPlane = false;
                    return;
                }

                FolderPlane newPlane = new FolderPlane();
                newPlane.SetFolderPlane(selectedPath);


                // UseCurrentPlane is set to true elsewhere ... when we move up or down a folder
                // Design Choice now: replace current SelectedFolder in FolderPlanes by newPlane
                if (UseCurrentPlane)
                {
                    UseCurrentPlane = false;
                    indexInPlanes = GetIndexFolderPlanes(SelectedFolderPlane.FullPathName);

                    if (indexInPlanes != -1) { FolderPlanes[indexInPlanes] = newPlane; } else FolderPlanes.Add(newPlane);                   
                }
                else
                {
                    FolderPlanes.Add(newPlane);
                }

                SelectedFolderPlane = newPlane;

            }
        }
        
        private FolderPlane selectedFolderPlane;
        public FolderPlane SelectedFolderPlane
        {
            get { return selectedFolderPlane ?? (selectedFolderPlane = new FolderPlane()); }
            set
            {
                selectedFolderPlane = value;

                // Sync with SelectedPath, main work done in Setter SelectedPath
                if (selectedFolderPlane != null)
                    if (SelectedPath != selectedFolderPlane.FullPathName) { SelectedPath = selectedFolderPlane.FullPathName; }

                RaisePropertyChanged("SelectedFolderPlane");
            }
        }

        private ObservableCollection<FolderPlane> folderPlanes;
        public ObservableCollection<FolderPlane> FolderPlanes
        {
            get { return folderPlanes ?? (folderPlanes = new ObservableCollection<FolderPlane>());}
        }

        private ObservableCollection<SavedFolderTabsItem> savedFolderTabs;
        public ObservableCollection<SavedFolderTabsItem> SavedFolderTabs
        {
            get { return savedFolderTabs ?? (savedFolderTabs = SavedFolderTabsUtils.Load()); }
            set
            {
                if (SetProperty(ref savedFolderTabs, value, "SavedFolderTabs"))
                {
                    // ..Add() does give a notification, but not a set, so at each .Add(new ..) do a save?? 
                    SavedFolderTabsUtils.Save(savedFolderTabs);
                }
            }
        }

        private int selectedIndexSavedFolderTabs;
        public int SelectedIndexSavedFolderTabs
        {
            get { return selectedIndexSavedFolderTabs; }
            set
            {
                if (SetProperty(ref selectedIndexSavedFolderTabs, value, "SelectedIndexSavedFolderTabs"))
                    if (value != -1)
                    {
                        SavedFolderTabsItem savedTabs;
                        savedTabs = SavedFolderTabs[value];

                        //Clear all FolderPlanes
                        FolderPlanes.Clear();

                        // Use logic in Setter SelectedPath to do the work/administration of making al the FolderPlanes 
                        // Empty first SelectedPath, so a first existing SelectedPath is never ignored 
                        SelectedPath = ""; 
                        for (int i = 0; i <= savedTabs.TabFullPathName.Count - 1; i++)
                        {
                            SelectedPath = savedTabs.TabFullPathName[i];
                        }

                        if (savedTabs.TabFullPathName.Count > 0) { SelectedPath = savedTabs.TabFullPathName[0]; }
                        SavedFolderTabsUtils.Save(savedFolderTabs);
                    }
            }
        }

        // ShaderOpacy binding is used to set Opacy/Shade in MainWindow when a popup is open
        private double shaderOpacity = 1.0;
        public double ShaderOpacity
        {
            get { return shaderOpacity; }
            set { SetProperty(ref shaderOpacity, value, "ShaderOpacity"); }
        }

        // this is a pattern, make it DRY
        private bool popup1IsOpen;
        public bool Popup1IsOpen
        {
            get { return popup1IsOpen; }
            set 
            { 
                SetProperty(ref popup1IsOpen, value, "Popup1IsOpen");
                if (value) { ShaderOpacity = 0.3; } else { ShaderOpacity = 1.0; }
            }
        }

        private bool popup2IsOpen;
        public bool Popup2IsOpen
        {
            get { return popup2IsOpen; }
            set
            {
                SetProperty(ref popup2IsOpen, value, "Popup2IsOpen");
                if (value) { ShaderOpacity = 0.3; } else { ShaderOpacity = 1.0; }
            }
        }

        // I see a certain pattern ....
        private bool popup3IsOpen;
        public bool Popup3IsOpen
        {
            get { return popup3IsOpen; }
            set
            {
                SetProperty(ref popup3IsOpen, value, "Popup3IsOpen");
                if (value) { ShaderOpacity = 0.3; } else { ShaderOpacity = 1.0; }
            }
        }

 

        // set by SelectedItemsChangedCommand on SelectedFolderPlane
        private List<String> selectedFolderItems;
        public List<String> SelectedFolderItems
        {
            get { return selectedFolderItems ?? (selectedFolderItems = new List<String>()); }
            set { SetProperty(ref selectedFolderItems, value, "SelectedFolderItems"); }
        }

        private List<String> snappedSelectedItems;
        public List<String> SnappedSelectedItems
        {
            get { return snappedSelectedItems ?? (snappedSelectedItems = new List<String>()); }
            set { SetProperty(ref snappedSelectedItems, value, "SnappedSelectedItems"); }
        }

        // This string will only contain filename without path, set in SelectedItemsChangedCommand
        private string selectedFolderItem;
        public String SelectedFolderItem
        {
            get { return selectedFolderItem  ?? (selectedFolderItem = ""); }            
            set { SetProperty(ref selectedFolderItem, value, "SelectedFolderItem"); }
        }

        // constructor
        public MainVm()
        {
            TabbedNavTrees = new TabbedNavTreesVm();
            TabbedNavTrees.SelectedNavTree = TabbedNavTrees.NavTrees[0];
            
            SelectedIndexSavedFolderTabs = -1;
        }
    }
}
