using System;
using Chronos.Core;
using System.Drawing;

namespace Chronos.DotNet.IISApplication
{
    internal class ProfilingTarget : IProfilingTarget
    {
        public ProfilingTarget()
        {
            Uid = new Guid("CFF10316-EAE7-41D7-B913-113A7A063E2A");
            TargetFrameworkUid = new Guid("{6711F85A-3CD1-4E76-8502-C3D4404E47C8}");
        }

        public Guid Uid { get; private set; }

        public Guid TargetFrameworkUid { get; private set; }

        public bool IsEnabled
        {
            get { return true; }
        }

        public string DisplayName
        {
            get { return Properties.Resources.DisplayName; }
        }

        public Bitmap Icon
        {
            get { return Properties.Resources.Icon; }
        }

        public IProfilingTargetController StartProfiling(ConfigurationSettings settings)
        {
            throw new NotImplementedException();
        }

        public bool CanStartProfiling(ConfigurationSettings settings, int processId)
        {
            throw new NotImplementedException();
        }
    }
}
