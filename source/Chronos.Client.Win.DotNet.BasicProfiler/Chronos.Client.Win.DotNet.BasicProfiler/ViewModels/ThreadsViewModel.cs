using System;
using System.Collections.Generic;
using Adenium;
using Chronos.Client.Win.ViewModels;
using Chronos.DotNet.BasicProfiler;
using Chronos.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler.ViewModels
{
    [ViewModelAttribute(Constants.ViewModels.ThreadsViewModel, Constants.Views.UnitsListView)]
    public sealed class ThreadsViewModel : UnitsListViewModel<ThreadInfo>
    {
        public ThreadsViewModel(IThreadCollection units)
            : base(units, GetColumns(), Constants.Menus.ThreadContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "Threads"; }
            set { }
        }

        private static IEnumerable<GridViewDynamicColumn> GetColumns()
        {
            return new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Id", "Uid", FilterById),
                new GridViewDynamicColumn("Name", "Name", FilterByName)
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

        private static bool FilterById(object item, string text)
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
