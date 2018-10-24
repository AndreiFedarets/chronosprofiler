using System;
using Adenium;
using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.Menu.DotNet.BasicProfiler
{
    internal sealed class CodeViewMenuItem : ProfilingMenuItemBase
    {
        public CodeViewMenuItem(IProfilingApplication application)
            : base(application)
        {
        }
        
        public override string Text
        {
            get { return Resources.CodeViewMenuItem_Text; }
            protected set { }
        }

        protected override IViewModel GetViewModel()
        {
            throw new NotImplementedException();
        }
    }
}
