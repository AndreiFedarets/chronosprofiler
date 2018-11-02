using System;
using System.Collections.Generic;
using Adenium;
using Chronos.Client.Win.ViewModels;
using Chronos.DotNet.BasicProfiler;
using Chronos.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler.ViewModels
{
    [ViewModelAttribute(Constants.ViewModels.AssembliesViewModel, Constants.Views.UnitsListView)]
    public sealed class AssembliesViewModel : UnitsListViewModel<AssemblyInfo>
    {
        public AssembliesViewModel(IAssemblyCollection units)
            : base(units, GetColumns(), Constants.Menus.AssemblyContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "Assemblies"; }
            set { }
        }

        private static IEnumerable<GridViewDynamicColumn> GetColumns()
        {
            return new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name", FilterByName),
                new GridViewDynamicColumn("AppDomain", "AppDomain.Name", FilterByAppDomain)
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

        private static bool FilterByAppDomain(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            AssemblyInfo assemblyInfo = (AssemblyInfo)item;
            AppDomainInfo appDomainInfo = assemblyInfo.AppDomain;
            if (appDomainInfo == null)
            {
                return true;
            }
            return appDomainInfo.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
