using Chronos.Client.Win.Common.ViewModels;
using Chronos.Common;
using Chronos.DotNet.BasicProfiler;
using Chronos.DotNet.ExceptionMonitor;
using Layex.Extensions;
using Layex.ViewModels;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.DotNet.ExceptionMonitor.ViewModels
{
    [ViewModel(Constants.ViewModels.ExceptionsViewModel, Constants.Views.UnitsListView)]
    public sealed class ExceptionsViewModel : UnitsListViewModel<ExceptionInfo>
    {
        public ExceptionsViewModel(IExceptionCollection units)
            : base(units, GetColumns(), Constants.Menus.ExceptionContentMenu)
        {
        }

        public override string DisplayName
        {
            get { return "Exceptions"; }
        }

        private static IEnumerable<GridViewDynamicColumn> GetColumns()
        {
            return new List<GridViewDynamicColumn>
            {
                new GridViewDynamicColumn("Exception Type", "ExceptionClass.Name", FilterByClass),
                new GridViewDynamicColumn("Exception Message", "Name", FilterByName)
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

        private static bool FilterByClass(object item, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            ExceptionInfo exceptionInfo = (ExceptionInfo)item;
            ClassInfo classInfo = exceptionInfo.ExceptionClass;
            if (classInfo == null)
            {
                return true;
            }
            return classInfo.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
