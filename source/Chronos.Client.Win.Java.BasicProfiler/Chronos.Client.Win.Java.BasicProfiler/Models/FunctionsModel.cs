using System.Collections;
using System.Collections.Generic;
using Chronos.Java.BasicProfiler;
using Chronos.Model;
using System;

namespace Chronos.Client.Win.Models.Java.BasicProfiler
{
    public class FunctionsModel : IUnitsModel
    {
        public FunctionsModel(IFunctionCollection units)
        {
            Units = units;
            Columns = new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name", FilterByName),
                new GridViewDynamicColumn("Full Name", "FullName", FilterByFullName),
                new GridViewDynamicColumn("Class", "Class.Name", FilterByClass)
            };
        }

        public string DisplayName
        {
            get { return "Functions"; }
        }

        public Type UnitType
        {
            get { return typeof(FunctionInfo); }
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
            FunctionInfo functionInfo = (FunctionInfo)item;
            return functionInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private bool FilterByFullName(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            FunctionInfo functionInfo = (FunctionInfo)item;
            return functionInfo.FullName.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private bool FilterByClass(object item, string text)
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
            return classInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
