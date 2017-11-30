using System.Collections;
using System.Collections.Generic;
using Chronos.Java.BasicProfiler;
using Chronos.Model;
using System;

namespace Chronos.Client.Win.Models.Java.BasicProfiler
{
    public class AssembliesModel : IUnitsModel
    {
        public AssembliesModel(IAssemblyCollection units)
        {
            Units = units;
            Columns = new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name", FilterByName),
                new GridViewDynamicColumn("AppDomain", "AppDomain.Name", FilterByAppDomain)
            };
        }

        public string DisplayName
        {
            get { return "Assemblies"; }
        }

        public Type UnitType
        {
            get { return typeof(AssemblyInfo); }
        }

        public IEnumerable<GridViewDynamicColumn> Columns { get; private set; }

        public IEnumerable Units { get; private set; }

        public UnitBase SelectedUnit { get; set; }

        private bool FilterByName(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            AssemblyInfo assemblyInfo = (AssemblyInfo)item;
            return assemblyInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private bool FilterByAppDomain(object item, string text)
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
            return appDomainInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
