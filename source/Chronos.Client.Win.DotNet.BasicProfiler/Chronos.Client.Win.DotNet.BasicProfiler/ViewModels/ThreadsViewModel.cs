using Chronos.Client.Win.Common.ViewModels;
using Chronos.Common;
using Chronos.DotNet.BasicProfiler;
using Layex.Extensions;
using Layex.ViewModels;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.DotNet.BasicProfiler.ViewModels
{
    [ViewModel(Constants.ViewModels.ThreadsViewModel, Constants.Views.UnitsListView)]
    public sealed class ThreadsViewModel : UnitsListViewModel<ThreadInfo>
    {
        public ThreadsViewModel(IThreadCollection units)
            : base(units, GetColumns(), Constants.Menus.ThreadContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "Threads"; }
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
