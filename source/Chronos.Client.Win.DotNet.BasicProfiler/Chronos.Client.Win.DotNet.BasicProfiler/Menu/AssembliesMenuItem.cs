﻿using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Menu
{
    internal sealed class AssembliesMenuItem : UnitsMenuItemBase
    {
        public AssembliesMenuItem(IProfilingApplication application)
            : base(application)
        {
        }
        
        public override string GetText()
        {
            return Resources.AssembliesMenuItem_Text;
        }

        protected override IUnitsListModel GetModel()
        {
            IAssemblyCollection collection = Application.ServiceContainer.Resolve<IAssemblyCollection>();
            AssembliesModel model = new AssembliesModel(collection);
            return model;
        }
    }
}
