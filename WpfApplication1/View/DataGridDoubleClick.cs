using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using WpfApplication1.Model;

// Execute a command when row of datagrid is doubleclicked, no code behind (should be part of standard datagrid)
// Example of an attached property

// Solution from user Lukas
// http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/632ea875-a5b8-4d47-85b3-b30f28e0b827

// XAML example:
//
//<Grid DataContext="{StaticResource viewModel}">
//  <DataGrid AutoGenerateColumns="True" 
//     ItemsSource="{Binding Data}" 
//     SelectedItem="{Binding SelectedItem}" 
//     clr:DataGridDoubleClickBehaviour.DoubleClickCommandCommand="{Binding ICommandVm}"
//   />
//</Grid>

// (Note: there seems also some default problems when dragging selected items, mousedown messes up selection (duh!), not this module)

namespace WpfApplication1.View
{
    public static class DataGridDoubleClick
    {
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached(
                                  "DoubleClickCommand", 
                                  typeof(ICommand), 
                                  typeof(DataGridDoubleClick),
                                  new PropertyMetadata(new PropertyChangedCallback(AttachOrRemoveDoubleClickCommandEvent)));

        //public static ICommand GetDoubleClickCommand(DependencyObject obj)
        //{
        //    return (ICommand)obj.GetValue(DoubleClickCommandProperty);
        //}

        // For binding XAML-ModelView
        public static void SetDoubleClickCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DoubleClickCommandProperty, value);
        }

        // Add/remove DoubleClick
        public static void AttachOrRemoveDoubleClickCommandEvent(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DataGrid dataGrid = obj as DataGrid;
            if (dataGrid != null)
            {
                ICommand cmd = (ICommand)args.NewValue;

                // OnAttach
                if (args.OldValue == null && args.NewValue != null)
                {
                    dataGrid.MouseDoubleClick += ExecuteDataGridDoubleClick;
                }
                // OnDetach
                else if (args.OldValue != null && args.NewValue == null)
                {
                    dataGrid.MouseDoubleClick -= ExecuteDataGridDoubleClick;
                }
            }
        }

        private static void ExecuteDataGridDoubleClick(object sender, MouseButtonEventArgs args)
        {
            DependencyObject obj = sender as DependencyObject;
            ICommand cmd = (ICommand)obj.GetValue(DoubleClickCommandProperty);

            if (cmd != null)
            {
                // Execute ICommand bound to DoubleClickCommand
                // Datagrid.CurrentItem known, in VM we translate to FolderPathItem
                object currentItem = ((sender as DataGrid).CurrentItem);  
                if (cmd.CanExecute(currentItem))
                {
                    cmd.Execute(currentItem);
                }
            }
        }
    }
}
