using System.Collections;
using System.Collections.Generic;
using Chronos.Java.BasicProfiler;
using Chronos.Model;
using System;

namespace Chronos.Client.Win.Models.Java.BasicProfiler
{
    public sealed class AppDomainsModel : IUnitsModel
    {
        public AppDomainsModel(IAppDomainCollection units)
        {
            Units = units;
            Columns = new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Name", "Name", FilterByName)
            };
        }

        public string DisplayName
        {
            get { return "AppDomains"; }
        }

        public Type UnitType
        {
            get { return typeof(AppDomainInfo); }
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
            AppDomainInfo appDomainInfo = (AppDomainInfo)item;
            return appDomainInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
