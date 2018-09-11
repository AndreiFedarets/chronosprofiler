using System;
using System.Windows;
using System.Windows.Controls;

namespace Chronos.Client.Win.Controls
{
    [TemplatePart(Name = MinimizeButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = RestoreButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = MaximizeButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = CloseButtonPartName, Type = typeof(Button))]
    public class CustomWindow : Window
    {
        private const string MinimizeButtonPartName = "MinimizeButton";
        private const string RestoreButtonPartName = "RestoreButton";
        private const string MaximizeButtonPartName = "MaximizeButton";
        private const string CloseButtonPartName = "CloseButton";
        public static readonly DependencyProperty CanMaximizeProperty;
        public static readonly DependencyProperty CanMinimizeProperty;

        private Button _minimizeButton;
        private Button _restoreButton;
        private Button _maximizeButton;
        private Button _closeButton;

        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
            CanMaximizeProperty = DependencyProperty.Register("CanMaximize", typeof(bool), typeof(CustomWindow), new PropertyMetadata(true, OnCanMaximizePropertyChanged));
            CanMinimizeProperty = DependencyProperty.Register("CanMinimize", typeof(bool), typeof(CustomWindow), new PropertyMetadata(true, OnCanMinimizePropertyChanged));
        }

        public CustomWindow()
        {
            SynchronizeWithWindowState();
        }

        public bool CanMaximize
        {
            get { return (bool)GetValue(CanMaximizeProperty); }
            set { SetValue(CanMaximizeProperty, value); }
        }

        public bool CanMinimize
        {
            get { return (bool)GetValue(CanMinimizeProperty); }
            set { SetValue(CanMinimizeProperty, value); }
        }

        protected override void OnInitialized(EventArgs e)
        {
            SourceInitialized += OnSourceInitialized;
            base.OnInitialized(e);
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            SynchronizeWithWindowState();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //MinimizeButton
            if (_minimizeButton != null)
            {
                _minimizeButton.Click -= OnMinimizeButtonClick;
            }
            _minimizeButton = GetTemplateChild(MinimizeButtonPartName) as Button;
            if (_minimizeButton != null)
            {
                _minimizeButton.Click += OnMinimizeButtonClick;
            }
            //RestoreButton
            if (_restoreButton != null)
            {
                _restoreButton.Click -= OnRestoreButtonClick;
            }
            _restoreButton = GetTemplateChild(RestoreButtonPartName) as Button;
            if (_restoreButton != null)
            {
                _restoreButton.Click += OnRestoreButtonClick;
            }
            //MaximizeButton
            if (_maximizeButton != null)
            {
                _maximizeButton.Click -= OnMaximizeButtonClick;
            }
            _maximizeButton = GetTemplateChild(MaximizeButtonPartName) as Button;
            if (_maximizeButton != null)
            {
                _maximizeButton.Click += OnMaximizeButtonClick;
            }
            //CloseButton
            if (_closeButton != null)
            {
                _closeButton.Click -= OnCloseButtonClick;
            }
            _closeButton = GetTemplateChild(CloseButtonPartName) as Button;
            if (_closeButton != null)
            {
                _closeButton.Click += OnCloseButtonClick;
            }

            SynchronizeWithWindowState();
            UpdateMaximizeButtonVisibility();
            UpdateMinimizeButtonVisibility();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            //base.OnStateChanged(e);
            SynchronizeWithWindowState();
        }

        private void SynchronizeWithWindowState()
        {
            switch (WindowState)
            {
                case WindowState.Maximized:
                    if (_restoreButton != null && CanMaximize)
                    {
                        _restoreButton.Visibility = Visibility.Visible;
                    }
                    if (_maximizeButton != null && CanMaximize)
                    {
                        _maximizeButton.Visibility = Visibility.Collapsed;
                    }
                    break;
                case WindowState.Normal:
                    if (_restoreButton != null && CanMaximize)
                    {
                        _restoreButton.Visibility = Visibility.Collapsed;
                    }
                    if (_maximizeButton != null && CanMaximize)
                    {
                        _maximizeButton.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnRestoreButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            WindowState = WindowState.Normal;
        }

        private void OnMaximizeButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            WindowState = WindowState.Maximized;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void UpdateMaximizeButtonVisibility()
        {
            Visibility visibility = CanMaximize ? Visibility.Visible : Visibility.Collapsed;
            if (_restoreButton != null)
            {
                _restoreButton.Visibility = visibility;
            }
            if (_maximizeButton != null)
            {
                _maximizeButton.Visibility = visibility;
            }
            SynchronizeWithWindowState();
        }

        private void UpdateMinimizeButtonVisibility()
        {
            Visibility visibility = CanMinimize ? Visibility.Visible : Visibility.Collapsed;
            if (_minimizeButton != null)
            {
                _minimizeButton.Visibility = visibility;
            }
            SynchronizeWithWindowState();
        }

        private static void OnCanMaximizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CustomWindow customWindow = (CustomWindow)sender;
            customWindow.UpdateMaximizeButtonVisibility();
        }

        private static void OnCanMinimizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CustomWindow customWindow = (CustomWindow)sender;
            customWindow.UpdateMinimizeButtonVisibility();
        }
    }
}