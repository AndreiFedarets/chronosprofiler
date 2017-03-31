using System.Collections;
using System.Collections.Generic;
using Chronos.DotNet.BasicProfiler;
using Chronos.Model;
using System;

namespace Chronos.Client.Win.Models.DotNet.BasicProfiler
{
    public class ThreadsModel : IUnitsModel
    {
        public ThreadsModel(IThreadCollection units)
        {
            Units = units;
            Columns = new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Id", "Uid", FilterById),
                new GridViewDynamicColumn("Name", "Name", FilterByName)
            };
        }

        public string DisplayName
        {
            get { return "Threads"; }
        }

        public Type UnitType
        {
            get { return typeof(ThreadInfo); }
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
            ThreadInfo threadInfo = (ThreadInfo)item;
            return threadInfo.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private bool FilterById(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            ulong id;
            if (!ulong.TryParse(text, out id))
            {
                return true;
            }
            ThreadInfo threadInfo = (ThreadInfo)item;
            return threadInfo.Uid == id;
        }
    }
}
