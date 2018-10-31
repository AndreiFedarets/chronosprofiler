using System;
using System.Windows;
using System.Windows.Controls;
using Adenium.ViewModels;

namespace Adenium
{
    public class View : UserControl
    {
        public static readonly DependencyProperty DisplayPanelProperty;
        public static readonly DependencyProperty ViewPositionProperty;
        public static readonly DependencyProperty AttachToProperty;
        public static readonly DependencyProperty ContentMinHeightProperty;
        public static readonly DependencyProperty ContentHeightProperty;
        public static readonly DependencyProperty ContentMaxHeightProperty;
        public static readonly DependencyProperty ContentMinWidthProperty;
        public static readonly DependencyProperty ContentWidthProperty;
        public static readonly DependencyProperty ContentMaxWidthProperty;
        private IViewBehaviorExtension _behaviorExtension;

        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
            DisplayPanelProperty = DependencyProperty.Register("DisplayPanel", typeof(bool), typeof(View), new FrameworkPropertyMetadata(false));
            ViewPositionProperty = DependencyProperty.Register("ViewPosition", typeof(Position), typeof(View), new FrameworkPropertyMetadata(Position.Default));
            AttachToProperty = DependencyProperty.Register("AttachTo", typeof(string), typeof(View), new FrameworkPropertyMetadata(string.Empty));

            ContentMinHeightProperty = DependencyProperty.Register("ContentMinHeight", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
            ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
            ContentMaxHeightProperty = DependencyProperty.Register("ContentMaxHeight", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));

            ContentMinWidthProperty = DependencyProperty.Register("ContentMinWidth", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
            ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
            ContentMaxWidthProperty = DependencyProperty.Register("ContentMaxWidth", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
        }

        public View()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
            DataContextChanged += OnDataContextChanged;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)DataContext; }
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

        public string AttachTo
        {
            get { return (string)GetValue(AttachToProperty); }
            set { SetValue(AttachToProperty, value); }
        }

        public double ContentMinHeight
        {
            get { return (double)GetValue(ContentMinHeightProperty); }
            set { SetValue(ContentMinHeightProperty, value); }
        }

        public double ContentHeight
        {
            get { return (double)GetValue(ContentHeightProperty); }
            set { SetValue(ContentHeightProperty, value); }
        }

        public double ContentMaxHeight
        {
            get { return (double)GetValue(ContentMaxHeightProperty); }
            set { SetValue(ContentMaxHeightProperty, value); }
        }

        public double ContentMinWidth
        {
            get { return (double)GetValue(ContentMinWidthProperty); }
            set { SetValue(ContentMinWidthProperty, value); }
        }

        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        public double ContentMaxWidth
        {
            get { return (double)GetValue(ContentMaxWidthProperty); }
            set { SetValue(ContentMaxWidthProperty, value); }
        }

        private void UpdateBehaviorExtension()
        {
            if (_behaviorExtension != null)
            {
                _behaviorExtension.Dispose();
            }
            if (DataContext is TabViewModel)
            {
                _behaviorExtension = new TabViewBehaviorExtension(this, (TabViewModel)DataContext);
            }
            else if (DataContext is GridViewModel)
            {
                _behaviorExtension = new GridViewBehaviorExtension(this, (GridViewModel)DataContext);
            }
            else if (DataContext is PlaceholderViewModel)
            {
                _behaviorExtension = new PlaceholderViewBehaviorExtension(this, (PlaceholderViewModel) DataContext);
            }
            else if (DataContext is ViewModel)
            {
                _behaviorExtension = new ContentViewBehaviorExtension();
            }
            else
            {
                throw new NotSupportedException("This type of DataContext is not supported");
            }
            _behaviorExtension.Initialize();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            View view = (View) sender;
            view.UpdateBehaviorExtension();
        }
    }
}
