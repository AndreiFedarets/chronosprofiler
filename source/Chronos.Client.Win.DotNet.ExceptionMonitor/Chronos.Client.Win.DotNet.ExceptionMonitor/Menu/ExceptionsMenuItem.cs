﻿using Chronos.Client.Win.DotNet.ExceptionMonitor.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.ExceptionMonitor;
using Chronos.DotNet.ExceptionMonitor;

namespace Chronos.Client.Win.DotNet.ExceptionMonitor.Menu
{
    internal sealed class ExceptionsMenuItem : UnitsMenuItemBase
    {
        public ExceptionsMenuItem(IProfilingApplication application)
            : base(application)
        {
        }

        public override string GetText()
        {
            return Resources.ManagedExceptionsMenuItem_Text;
        }

        protected override IUnitsListModel GetModel()
        {
            IManagedExceptionCollection collection = Application.ServiceContainer.Resolve<IManagedExceptionCollection>();
            ManagedExceptionsModel model = new ManagedExceptionsModel(collection);
            return model;
        }
    }
}