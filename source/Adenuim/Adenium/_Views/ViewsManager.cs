using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Adenium.Layouting;
using Caliburn.Micro;

namespace Adenium
{
    internal static class ViewsManager
    {
        public const string ViewContentPartName = "ViewContent";
        private static readonly Func<object, DependencyObject, object, UIElement> LocateForModel;

        static ViewsManager()
        {
            LocateForModel = ViewLocator.LocateForModel;
            ViewLocator.LocateForModel = LocateForModelPrivate;
        }

        public static View LocateViewForModel(object viewModel)
        {
            UIElement viewElement = ViewLocator.LocateForModel(viewModel, null, null);
            View view = (View) viewElement;
            ViewModelBinder.Bind(viewModel, view, null);
            return view;
        }

        public static ContentControl FindViewContent(View view)
        {
            ContentControl contentControl = view.FindFirstChild<ContentControl>(ViewContentPartName);
            if (contentControl == null)
            {
                contentControl = view;
            }
            return contentControl;
        }

        private static UIElement LocateForModelPrivate(object viewModel, DependencyObject dependencyObject, object context)
        {
            EnsureViewModelAssembly(viewModel);
            ViewModelBuilder.Build(viewModel);
            return LocateForModel(viewModel, dependencyObject, context);
        }

        private static void EnsureViewModelAssembly(object viewModel)
        {
            Type viewModelType = viewModel.GetType();
            Assembly assembly = viewModelType.Assembly;
            if (!AssemblySource.Instance.Contains(assembly) && assembly.IsWPFAssembly())
            {
                AssemblySource.Instance.Add(assembly);
            }
        }
    }
}
