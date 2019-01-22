using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using WpfApplication1.Model;
using System.Windows.Media;
using System.Collections;
using System.Windows.Documents;

// Rearanging,sorting 1 MVVM listbox by DnD
// XAML example:  <ListBox clr:DnDSortList.Attach1="True"/>

// Example of an attached property (= Wimpie's 1st attached property)
// Rather simplistic drag and drop, partly copied from rudigrobler. Just a try, do not use this as a reference!!
// Works only if listboxes are involved!!!!!

// See for drag and drop:
// rudigrobler:            http://www.codeproject.com/Articles/22855/ListBox-Drag-Drop-using-Attached-Properties
// Bea Stollnitz:          http://bea.stollnitz.com/blog/?p=53
// LesterLobo/Pavan Podila http://blogs.msdn.com/b/llobo/archive/2006/12/08/drag-drop-library.aspx
// Philipp Sumi            http://www.hardcodet.net/2009/03/moving-data-grid-rows-using-drag-and-drop

// to do:
// visual feedback using a specified popup, copy original element, using DataTemplate
  
namespace WpfApplication1.View
{
    public static class DnDSortOneListBox
    {

        // An attached property Boolean Attach1 is registered
        public static readonly DependencyProperty Attach1Property = DependencyProperty.RegisterAttached(
                                  "Attach1",
                                  typeof(Boolean),
                                  typeof(DnDSortOneListBox),
                                  new FrameworkPropertyMetadata(OnAttach1ChangeCallBack));

        // For binding XAML-ModelView. Only setter.
        public static void SetAttach1(DependencyObject obj, Boolean value)
        {
            obj.SetValue(Attach1Property, value);
        }

        // OnAttach/ OnDetach hook/unhook events
        public static void OnAttach1ChangeCallBack(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            listBox1 = obj as ListBox;
            //UIElement sourceUIE = obj as UIElement;
            if (listBox1 != null)
            {
                // OnAttach
                if ((!(bool)args.OldValue) && ((bool)args.NewValue))
                {
                    // We have now succesfully attached the object....

                    // Drag:
                    listBox1.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(listBox_PreviewMouseLeftButtonDown);
                    listBox1.PreviewMouseMove += new MouseEventHandler(listBox_PreviewMouseMove);
                    listBox1.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(listBox_PreviewMouseLeftButtonUp);

                    // Drop
                    listBox1.AllowDrop = true;
                    listBox1.PreviewDragEnter += new DragEventHandler(listBox_PreviewDragEnter);
                    listBox1.PreviewDragOver += new DragEventHandler(listBox_PreviewDragOver);
                    listBox1.PreviewDragLeave += new DragEventHandler(listBox_PreviewDragLeave);
                    listBox1.PreviewDrop += new DragEventHandler(listBox_PreViewDrop);
                }
                // OnDetach
                else if (((bool)args.OldValue) && (!(bool)args.NewValue))
                {
                    // Drag:
                    listBox1.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(listBox_PreviewMouseLeftButtonDown);
                    listBox1.PreviewMouseMove -= new MouseEventHandler(listBox_PreviewMouseMove);
                    listBox1.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(listBox_PreviewMouseLeftButtonUp);

                    // Drop
                    listBox1.PreviewDragEnter -= new DragEventHandler(listBox_PreviewDragEnter);
                    listBox1.PreviewDragOver -= new DragEventHandler(listBox_PreviewDragOver);
                    listBox1.PreviewDragLeave -= new DragEventHandler(listBox_PreviewDragLeave);
                    listBox1.PreviewDrop -= new DragEventHandler(listBox_PreViewDrop);
                }
            }
        }


        // Primitive drag drop. 
        static private ListBox listBox1;
        static private Type itemType;
        // dummyX to mark no drag
        const int dummyX = -1010;
        static Point startPoint = new Point(dummyX, dummyX);


        private static void listBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private static void listBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            startPoint.X = dummyX;
        }

        private static void listBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (startPoint.X == dummyX) return;

            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Houston, the dragging has started, just sit down
                startPoint.X = dummyX;

                listBox1 = (ListBox)sender;
                object data = (object)GetObjectDataFromPoint(listBox1, e.GetPosition(listBox1));
                int targetIdx1 = listBox1.Items.IndexOf(data);

                if (data != null)
                {
                    itemType = data.GetType();
                    DragDrop.DoDragDrop(listBox1, data, DragDropEffects.Move);
                }
            }
        }

        private static void listBox_PreviewDragLeave(object sender, DragEventArgs e)
        {
        }

        private static void listBox_PreviewDragEnter(object sender, DragEventArgs e)
        {

        }

        private static void listBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            ListBox listBox2 = (ListBox)sender;
            if (listBox1 != listBox2)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private static void listBox_PreViewDrop(object sender, DragEventArgs e)
        {
            listBox1 = (ListBox)sender;
            object data = e.Data.GetData(itemType);
            int indexDrag = listBox1.Items.IndexOf(data);

            object data2 = (object)GetObjectDataFromPoint(listBox1, e.GetPosition(listBox1));
            int indexDrop = listBox1.Items.IndexOf(data2);

            if ((indexDrag == indexDrop) ||
                (indexDrag < 0) || (indexDrag > listBox1.Items.Count - 1) ||
                (indexDrop < 0) || (indexDrop > listBox1.Items.Count - 1)) return;

            IEnumerable itemsSource = listBox1.ItemsSource;

            // Wimpie's principle: Drop always other side indexDrop
            if (indexDrop < 0) indexDrop = 0;
            if (indexDrop > listBox1.Items.Count - 1) indexDrop = listBox1.Items.Count - 1;

            ((IList)itemsSource).RemoveAt(indexDrag);
            ((IList)itemsSource).Insert(indexDrop, data);
            listBox1.SelectedIndex = indexDrop;

            e.Handled = true;
        }

        private static object GetObjectDataFromPoint(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                //get the object from the element
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    // try to get the object value for the corresponding element
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    //get the parent and we will iterate again
                    if (data == DependencyProperty.UnsetValue)
                        element = VisualTreeHelper.GetParent(element) as UIElement;

                    //if we reach the actual listbox then we must break to avoid an infinite loop
                    if (element == source)
                        return null;
                }

                //return the data that we fetched only if it is not Unset value, 
                //which would mean that we did not find the data
                if (data != DependencyProperty.UnsetValue)
                    return data;
            }

            return null;
        }
    }
}
