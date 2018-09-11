using System.Collections;
using System.Collections.Generic;
using Chronos.Java.BasicProfiler;
using Chronos.Model;
using System;

namespace Chronos.Client.Win.Models.Java.BasicProfiler
{
    public class ClassesModel : IUnitsModel
    {
        public ClassesModel(IClassCollection units)
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
            get { return "Classes"; }
        }

        public Type UnitType
        {
            get { return typeof(ClassInfo); }
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
            ClassInfo classInfo = (ClassInfo)item;
            return classInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private bool FilterByAssembly(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            ClassInfo classInfo = (ClassInfo)item;
            AssemblyInfo assemblyInfo = classInfo.Assembly;
            if (assemblyInfo == null)
            {
                return true;
            }
            return assemblyInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
