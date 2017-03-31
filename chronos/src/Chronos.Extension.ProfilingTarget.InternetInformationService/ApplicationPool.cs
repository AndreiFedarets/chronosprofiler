using Microsoft.Web.Administration;
using System.Threading;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService
{
	internal class ApplicationPool : IApplicationPool
	{
		private readonly Microsoft.Web.Administration.ApplicationPool _applicationPool;

		public ApplicationPool(Microsoft.Web.Administration.ApplicationPool applicationPool)
		{
			_applicationPool = applicationPool;
		}

		public string Name
		{
			get { return _applicationPool.Name; }
		}

        public void Restart()
        {
            WaitWhileState(ObjectState.Starting);
            if (_applicationPool.State == ObjectState.Started)
            {
                _applicationPool.Stop();
            }
            WaitWhileState(ObjectState.Stopping);
            _applicationPool.Start();
            WaitWhileState(ObjectState.Starting);
        }

        public void WaitWhileState(ObjectState state)
        {
            while (_applicationPool.State == state)
            {
                Thread.Sleep(10);
            }
        }
	}
}
