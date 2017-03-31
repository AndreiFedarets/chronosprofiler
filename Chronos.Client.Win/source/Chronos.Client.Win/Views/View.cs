using System;
using System.Windows;
using System.Windows.Controls;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.Views
{
    public class View : UserControl
    {
        public static readonly DependencyProperty DisplayPanelProperty;
        public static readonly DependencyProperty ViewPositionProperty;
        public static readonly DependencyProperty AttachToProperty;

        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
            DisplayPanelProperty = DependencyProperty.Register("DisplayPanel", typeof(bool), typeof(View), new FrameworkPropertyMetadata(false, null));
            ViewPositionProperty = DependencyProperty.Register("ViewPosition", typeof(Position), typeof(View), new FrameworkPropertyMetadata(Position.Default, null));
            AttachToProperty = DependencyProperty.Register("AttachTo", typeof(Guid), typeof(View), new FrameworkPropertyMetadata(Guid.Empty, null));
        }

        public ViewModel ViewModel
        {
            get { return (ViewModel)DataContext; }
        }

        public bool DisplayPanel
        {
            get { return (bool)GetValue(DisplayPanelProperty); }
            set { SetValue(DisplayPanelProperty, value); }
        }

        public Position ViewPosition
        {
            get { return (Position)GetValue(ViewPositionProperty); }
            set { SetValue(ViewPositionProperty, value); }
        }

        public Guid AttachTo
        {
            get { return (Guid)GetValue(AttachToProperty); }
            set { SetValue(AttachToProperty, value); }
        }
    }
}
