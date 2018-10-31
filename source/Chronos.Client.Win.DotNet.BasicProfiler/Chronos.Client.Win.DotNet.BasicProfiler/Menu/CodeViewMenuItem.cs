using System;
using Adenium;
using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Menu
{
    internal sealed class CodeViewMenuItem : ProfilingMenuItemBase
    {
        public CodeViewMenuItem(IProfilingApplication application)
            : base(application)
        {
        }
        
        public override string GetText()
        {
            return Resources.CodeViewMenuItem_Text;
        }

        protected override IViewModel GetViewModel()
        {
            throw new NotImplementedException();
        }
    }
}
