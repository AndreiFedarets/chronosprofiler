using Chronos.Client.Win.Common.ViewModels;
using Chronos.Common;
using Chronos.DotNet.BasicProfiler;
using Layex.Extensions;
using Layex.ViewModels;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.DotNet.BasicProfiler.ViewModels
{
    [ViewModel(Constants.ViewModels.ModulesViewModel, Constants.Views.UnitsListView)]
    public sealed class ModulesViewModel : UnitsListViewModel<ModuleInfo>
    {
        public ModulesViewModel(IModuleCollection units)
            : base(units, GetColumns(), Constants.Menus.ModuleContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "Modules"; }
        }

        private static IEnumerable<GridViewDynamicColumn> GetColumns()
        {
            return new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name", FilterByName),
                new GridViewDynamicColumn("Assembly", "Assembly.Name", FilterByAssembly)
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

        private static bool FilterByAssembly(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            ModuleInfo moduleInfo = (ModuleInfo)item;
            AssemblyInfo assemblyInfo = moduleInfo.Assembly;
            if (assemblyInfo == null)
            {
                return true;
            }
            return assemblyInfo.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
