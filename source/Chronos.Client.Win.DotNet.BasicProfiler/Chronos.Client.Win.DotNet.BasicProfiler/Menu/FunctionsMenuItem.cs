﻿using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Menu
{
    internal sealed class FunctionsMenuItem : UnitsMenuItemBase
    {
        public FunctionsMenuItem(IProfilingApplication application)
            : base(application)
        {
        }
        
        public override string GetText()
        {
            return Resources.FunctionsMenuItem_Text;
        }

        protected override IUnitsListModel GetModel()
        {
            IFunctionCollection collection = Application.ServiceContainer.Resolve<IFunctionCollection>();
            FunctionsModel model = new FunctionsModel(collection);
            return model;
        }
    }
}
