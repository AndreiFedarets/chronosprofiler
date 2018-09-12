using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;

namespace Chronos.Proxy
{
    public class DefaultApplicationSponsor : MarshalByRefObject, IApplicationSponsor, ISponsor
    {
        public TimeSpan Renewal(ILease lease)
        {
            Debug.Assert(lease.CurrentState == LeaseState.Active);
            return Constants.Remoting.SponsorshipTimeout;
        }

        public void Register(object obj)
        {
            MarshalByRefObject remoteObject = obj as MarshalByRefObject;
            if (remoteObject == null)
            {
                return;
            }
            ILease lease = RemotingServices.GetLifetimeService(remoteObject) as ILease;
            if (lease == null)
            {
                return;
            }
            lease.Register(this);
        }

        public void Unregister(object obj)
        {
            MarshalByRefObject remoteObject = obj as MarshalByRefObject;
            if (remoteObject == null)
            {
                return;
            }
            ILease lease = RemotingServices.GetLifetimeService(remoteObject) as ILease;
            if (lease == null)
            {
                return;
            }
            lease.Unregister(this);
        }
    }
}
