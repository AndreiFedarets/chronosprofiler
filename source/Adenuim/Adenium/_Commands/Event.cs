using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Adenium
{
    public static class Event
    {
        private const string ClickEventName = "Click";
        private const string MouseDoubleClickEventName = "MouseDoubleClick";
        private static readonly IDictionary<string, Type> Behaviors;

        private static readonly DependencyProperty EventCommandBehaviorProperty =
            DependencyProperty.RegisterAttached("EventCommandBehavior", typeof (ControlEventCommandBehavior), typeof (Event), null);

        public static readonly DependencyProperty EventNameProperty = DependencyProperty.RegisterAttached("EventName",
            typeof (string), typeof (Event), new PropertyMetadata(OnEventNameChanged));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command",
            typeof (ICommand), typeof (Event), new PropertyMetadata(OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof (object), typeof (Event),
                new PropertyMetadata(OnCommandParameterPropertyChanged));

        static Event()
        {
            Behaviors = new Dictionary<string, Type>();
            Behaviors.Add(ClickEventName, typeof (ControlClickCommandBehavior));
            Behaviors.Add(MouseDoubleClickEventName, typeof (ControlDoubleClickCommandBehavior));
        }

        //Command
        public static void SetCommand(Control control, ICommand command)
        {
            control.SetValue(CommandProperty, command);
        }

        public static ICommand GetCommand(Control control)
        {
            return control.GetValue(CommandProperty) as ICommand;
        }

        private static void OnCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Control control = dependencyObject as Control;
            if (control != null)
            {
                ControlEventCommandBehavior behavior = GetOrCreateBehavior(control);
                behavior.Command = (e.NewValue as ICommand);
            }
        }

        //Command Parameter
        public static void SetCommandParameter(Control control, object parameter)
        {
            control.SetValue(CommandParameterProperty, parameter);
        }

        public static object GetCommandParameter(Control control)
        {
            return control.GetValue(CommandParameterProperty);
        }

        private static void OnCommandParameterPropertyChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Control control = dependencyObject as Control;
            if (control != null)
            {
                ControlEventCommandBehavior behavior = GetOrCreateBehavior(control);
                behavior.CommandParameter = e.NewValue;
            }
        }

        //Event Name
        public static void SetEventName(Control control, string eventName)
        {
            control.SetValue(EventNameProperty, eventName);
        }

        public static string GetEventName(Control control)
        {
            return (string) control.GetValue(EventNameProperty);
        }

        private static void OnEventNameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            //Control control = dependencyObject as Control;
            //if (control != null)
            //{
            //    ControlEventCommandBehavior behavior = GetOrCreateBehavior(control);
            //    behavior.Command = (e.NewValue as ICommand);
            //}
        }

        //Factory
        private static ControlEventCommandBehavior GetOrCreateBehavior(Control control)
        {
            string eventName = GetEventName(control);
            if (string.IsNullOrEmpty(eventName) || !Behaviors.ContainsKey(eventName))
            {
                eventName = ClickEventName;
            }
            ControlEventCommandBehavior behavior =
                (ControlEventCommandBehavior) control.GetValue(EventCommandBehaviorProperty);
            if (behavior == null)
            {
                Type behaviorType = Behaviors[eventName];
                behavior = (ControlEventCommandBehavior) Activator.CreateInstance(behaviorType, control);
                control.SetValue(EventCommandBehaviorProperty, behavior);
            }
            return behavior;
        }
    }
}
