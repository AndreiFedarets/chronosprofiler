﻿using System.Windows;
using System.Windows.Controls;

namespace Chronos.Client.Win.Controls
{
    internal class ContextMenu : System.Windows.Controls.ContextMenu
    {
        public static readonly DependencyProperty SourceProperty;

        static ContextMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(typeof(System.Windows.Controls.ContextMenu)));
            SourceProperty = DependencyProperty.Register("Source", typeof(Win.Menu.IMenu), typeof(ContextMenu), new PropertyMetadata(SourcePropertyChanged));
        }

        public Win.Menu.IMenu Source
        {
            get { return (Win.Menu.IMenu)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private void BindViewModel()
        {
            Items.Clear();
            if (Source == null)
            {
                return;
            }
            Visibility = Visibility.Collapsed;
            foreach (Win.Menu.IControl viewModel in Source)
            {
                BindViewModelRecurcive(this, viewModel);
                Visibility = Visibility.Visible;
            }
        }

        private void BindViewModelRecurcive(Control parent, Win.Menu.IControl childViewModel)
        {
            ItemsControl itemsControl = parent as ItemsControl;
            if (itemsControl == null)
            {
                return;
            }
            Control control = MenuControlConverter.Convert(childViewModel);
            itemsControl.Items.Add(control);
            Win.Menu.IControlCollection viewModelCollection = childViewModel as Win.Menu.IControlCollection;
            if (viewModelCollection == null)
            {
                return;
            }
            foreach (Win.Menu.IControl viewModel in viewModelCollection)
            {
                BindViewModelRecurcive(control, viewModel);
            }
        }

        private static void SourcePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ContextMenu menu = (ContextMenu)source;
            menu.BindViewModel();
        }

        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();
        //    //SEARCH_BUTTON_PART
        //    if (_browseButton != null)
        //    {
        //        _browseButton.Click -= OnBrowseButtonClick;
        //    }
        //    _browseButton = GetTemplateChild(BrowseButtonPartName) as Button;
        //    if (_browseButton != null)
        //    {
        //        _browseButton.Click += OnBrowseButtonClick;
        //    }
        //}
    }
}
