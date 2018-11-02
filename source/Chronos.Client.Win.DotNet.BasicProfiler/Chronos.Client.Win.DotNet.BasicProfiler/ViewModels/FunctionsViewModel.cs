using System;
using System.Collections.Generic;
using Adenium;
using Chronos.Client.Win.ViewModels;
using Chronos.DotNet.BasicProfiler;
using Chronos.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler.ViewModels
{
    [ViewModelAttribute(Constants.ViewModels.FunctionsViewModel, Constants.Views.UnitsListView)]
    public sealed class FunctionsViewModel : UnitsListViewModel<FunctionInfo>
    {
        public FunctionsViewModel(IFunctionCollection units)
            : base(units, GetColumns(), Constants.Menus.FunctionContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "Functions"; }
            set { }
        }

        private static IEnumerable<GridViewDynamicColumn> GetColumns()
        {
            return new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name", FilterByName),
                new GridViewDynamicColumn("Full Name", "FullName", FilterByFullName),
                new GridViewDynamicColumn("Class", "Class.Name", FilterByClass)
            };
        }

        private static bool FilterByName(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            UnitBase unit = (UnitBase)item;
            return unit.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool FilterByFullName(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            FunctionInfo functionInfo = (FunctionInfo)item;
            return functionInfo.FullName.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool FilterByClass(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            FunctionInfo functionInfo = (FunctionInfo)item;
            ClassInfo classInfo = functionInfo.Class;
            if (classInfo == null)
            {
                return true;
            }
            return classInfo.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
