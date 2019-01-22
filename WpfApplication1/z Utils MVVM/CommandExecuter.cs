using System.Windows.Input;
using System.Reflection;
using System.Diagnostics;
using System.Windows;
using System;

// One event to command with parameter
// http://nerobrain.blogspot.com/2012/01/wpf-events-to-command.html
// code by Naveen

// XAML:
// <Datagrid mvvm:CommandExecuter.Command="{Binding DoSelectionChanged}" 
//           mvvm:CommandExecuter.OnEvent="OnSelectionChanged"
//           mvvm:CommandExecuter.Parameter="SelectionChanged" ></Datagrid>

// Note:
// If dll's are allowed Interaction.Triggers+EventTrigger from the Expression Blend SDK is the preferred solution
// I needed ABC (Attached Behaviour Commands) with parameter, 
// Google for Marlon Grechs or Sacha's Barber's take http://sachabarbs.wordpress.com/2009/05/02/wpf-attached-commands/
// Just took first code that did the job, did not examine this code ...
   
namespace MVVM
{
    public class CommandExecuter
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(CommandExecuter), new PropertyMetadata(CommandPropertyChangedCallback));

        public static readonly DependencyProperty OnEventProperty = DependencyProperty.RegisterAttached("OnEvent", typeof(string), typeof(CommandExecuter));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(CommandExecuter));

        public static void CommandPropertyChangedCallback(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            string onEvent = (string)depObj.GetValue(OnEventProperty);
            Debug.Assert(onEvent != null, "OnEvent must be set.");
            var eventInfo = depObj.GetType().GetEvent(onEvent);
            if (eventInfo != null)
            {
                var mInfo = typeof(CommandExecuter).GetMethod("OnRoutedEvent", BindingFlags.NonPublic | BindingFlags.Static);
                eventInfo.GetAddMethod().Invoke(depObj, new object[] { Delegate.CreateDelegate(eventInfo.EventHandlerType, mInfo) });
            }
            else
            {
                Debug.Fail(string.Format("{0} is not found on object {1}", onEvent, depObj.GetType()));
            }

        }

        public static void SetCommand(UIElement element, ICommand command)
        {
            element.SetValue(CommandProperty, command);
        }

        public static void SetOnEvent(UIElement element, string evnt)
        {
            element.SetValue(OnEventProperty, evnt);
        }

        public static void SetCommandParameter(UIElement element, object commandParam)
        {
            element.SetValue(CommandParameterProperty, commandParam);
        }

        private static void OnRoutedEvent(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement)sender;
            if (element != null)
            {
                ICommand command = element.GetValue(CommandProperty) as ICommand;
                if (command != null && command.CanExecute(element.GetValue(CommandParameterProperty)))
                {
                    command.Execute(element.GetValue(CommandParameterProperty));
                }
            }
        }
    }
}


