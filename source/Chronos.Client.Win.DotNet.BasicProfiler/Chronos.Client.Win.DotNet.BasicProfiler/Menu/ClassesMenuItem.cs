﻿using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Menu
{
    internal sealed class ClassesMenuItem : UnitsMenuItemBase
    {
        public ClassesMenuItem(IProfilingApplication application)
            : base(application)
        {
        }
        
        public override string GetText()
        {
            return Resources.ClassesMenuItem_Text;
        }

        protected override IUnitsListModel GetModel()
        {
            IClassCollection collection = Application.ServiceContainer.Resolve<IClassCollection>();
            ClassesModel model = new ClassesModel(collection);
            return model;
        }
    }
}
