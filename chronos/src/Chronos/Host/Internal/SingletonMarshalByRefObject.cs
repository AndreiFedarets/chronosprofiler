using System;

namespace Chronos.Host.Internal
{
	internal class SingletonMarshalByRefObject : MarshalByRefObject
	{
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
