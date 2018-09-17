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
        public static readonly DependencyProperty ContentMinHeightProperty;
        public static readonly DependencyProperty ContentHeightProperty;
        public static readonly DependencyProperty ContentMaxHeightProperty;
        public static readonly DependencyProperty ContentMinWidthProperty;
        public static readonly DependencyProperty ContentWidthProperty;
        public static readonly DependencyProperty ContentMaxWidthProperty;

        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
            DisplayPanelProperty = DependencyProperty.Register("DisplayPanel", typeof(bool), typeof(View), new FrameworkPropertyMetadata(false));
            ViewPositionProperty = DependencyProperty.Register("ViewPosition", typeof(Position), typeof(View), new FrameworkPropertyMetadata(Position.Default));
            AttachToProperty = DependencyProperty.Register("AttachTo", typeof(Guid), typeof(View), new FrameworkPropertyMetadata(Guid.Empty));

            ContentMinHeightProperty = DependencyProperty.Register("ContentMinHeight", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
            ContentHeightProperty = DependencyProperty.Register("ContentHeight", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
            ContentMaxHeightProperty = DependencyProperty.Register("ContentMaxHeight", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));

            ContentMinWidthProperty = DependencyProperty.Register("ContentMinWidth", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
            ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
            ContentMaxWidthProperty = DependencyProperty.Register("ContentMaxWidth", typeof(double), typeof(View), new FrameworkPropertyMetadata(double.NaN));
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

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }
    }
}
