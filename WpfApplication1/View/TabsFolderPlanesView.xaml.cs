using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1.View
{
    /// <summary>
    /// Interaction logic for TabbedFolderPlanesView.xaml
    /// </summary>
    public partial class TabsFolderPlanesView : UserControl
    {
        public TabsFolderPlanesView()
        {
            InitializeComponent();
        }
 
        private void FolderPlanesHeaders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Hack. 

            // Problem: 
            // If SelectedItem changes (In my case using the combobox button), correct header and FolderMap are selected 
            // however... the selected item is not brought into view if it is not visable already
            // Probably listbox virtualized or scrollviewer??
            // Note: Maybe a TabControl does work instead of a listbox. 

            ListBox lb = (sender as ListBox);
            if (lb != null)
            {
                // the old trick bring into view (focus, container stuff) alone did not work, UpdateLayout does the trick
                lb.ScrollIntoView(lb.SelectedItem);
                lb.UpdateLayout();
            }

        }


    }
}
