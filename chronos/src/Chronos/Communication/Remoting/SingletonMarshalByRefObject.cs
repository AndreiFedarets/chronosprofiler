using System;

namespace Chronos.Communication.Remoting
{
	public class SingletonMarshalByRefObject : MarshalByRefObject
	{
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
