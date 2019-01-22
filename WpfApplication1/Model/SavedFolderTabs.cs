using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace WpfApplication1.Model
{
    // Saving and loading SavedFolderTabs
    public class SavedFolderTabsItem
    {
        public string FriendlyName { get; set; }
        public Collection<String> TabFullPathName { get; set; }
    }

    public static class SavedFolderTabsUtils
    {
        public static void Save(ObservableCollection<SavedFolderTabsItem> SavedFolderTabs, string FileName = "FolderTabs.Xaml")
        {
            // Create a backup
            string backupName = Path.ChangeExtension(FileName, ".old");
            if (File.Exists(FileName))
            {
                if (File.Exists(backupName)) File.Delete(backupName);
                File.Move(FileName, backupName);
            }

            // Save SavedFolderTabs
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                XmlSerializer ser = new XmlSerializer(typeof(ObservableCollection<SavedFolderTabsItem>));
                ser.Serialize(fs, SavedFolderTabs);
                fs.Flush();
                fs.Close();
            }
        }

        public static ObservableCollection<SavedFolderTabsItem> Load(string FileName = "FolderTabs.Xaml")
        {
            ObservableCollection<SavedFolderTabsItem> SavedFolderTabs = new ObservableCollection<SavedFolderTabsItem> { };

            // if there is no file, return empty list
            if (!File.Exists(FileName))
            {
                return SavedFolderTabs;
            }

            // Load SavedFolderTabs
            using (FileStream fs = new FileStream(FileName, FileMode.Open))
            {
                XmlSerializer ser = new XmlSerializer(typeof(ObservableCollection<SavedFolderTabsItem>));
                SavedFolderTabs = (ObservableCollection<SavedFolderTabsItem>)ser.Deserialize(fs);
            }
            return SavedFolderTabs;
        }
    }
}
