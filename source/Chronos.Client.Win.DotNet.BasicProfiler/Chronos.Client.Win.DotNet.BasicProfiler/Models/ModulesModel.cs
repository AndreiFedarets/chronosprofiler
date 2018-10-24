using System.Collections;
using System.Collections.Generic;
using Chronos.DotNet.BasicProfiler;
using Chronos.Model;
using System;
using Adenium;

namespace Chronos.Client.Win.Models.DotNet.BasicProfiler
{
    public class ModulesModel : IUnitsListModel
    {
        public ModulesModel(IModuleCollection units)
        {
            Units = units;
            Columns = new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name", FilterByName),
                new GridViewDynamicColumn("Assembly", "Assembly.Name", FilterByAssembly)
            };
        }

        public string DisplayName
        {
            get { return "Modules"; }
        }

        public Type UnitType
        {
            get { return typeof(ModuleInfo); }
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
            ModuleInfo moduleInfo = (ModuleInfo)item;
            return moduleInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private bool FilterByAssembly(object item, string text)
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
            return assemblyInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
