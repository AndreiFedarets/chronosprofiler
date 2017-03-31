using System;
using System.Runtime.Remoting;

namespace Chronos.Proxy
{
    public abstract class ProxyBaseObject<T> : MarshalByRefObject, IProxyObject, IDisposable
    {
        private readonly DisposableTracker _disposableTracker;
        private readonly Guid _sponsorshipCookie;

        protected ProxyBaseObject(T remoteObject)
            : this(remoteObject, true)
        {
        }

        protected ProxyBaseObject(T remoteObject, bool resolveRealRemoteObject)
        {
            _disposableTracker = new DisposableTracker(this);
            if (resolveRealRemoteObject)
            {
                remoteObject = ProxyBaseObjectHelper.ResolveRealRemoteObject(remoteObject);
            }
            _sponsorshipCookie = InitializeSponsorship(remoteObject);
            RemoteObject = remoteObject;
        }

        protected T RemoteObject { get; private set; }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        object IProxyObject.GetRemoteObject()
        {
            return RemoteObject;
        }

        protected TResult Execute<TResult>(Func<TResult> func)
        {
            VerifyDisposed();
            try
            {
                return func();
            }
            catch (RemotingException remotingException)
            {
                throw new RemoteApplicationUnavailableException(remotingException);
            }
        }

        protected bool TryExecute(Action action)
        {
            VerifyDisposed();
            try
            {
                action();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void Execute(Action action)
        {
            VerifyDisposed();
            try
            {
                action();
            }
            catch (RemotingException remotingException)
            {
                throw new RemoteApplicationUnavailableException(remotingException);
            }
        }

        public virtual void Dispose()
        {
            VerifyDisposed();
            UninitializeSponsorship(RemoteObject, _sponsorshipCookie);
            _disposableTracker.Dispose();
        }

        protected void VerifyDisposed()
        {
            _disposableTracker.VerifyDisposed();
        }

        private Guid InitializeSponsorship(T remoteObject)
        {
            ILifetimeSponsor sponsor = this as ILifetimeSponsor;
            if (sponsor == null)
            {
                return Guid.Empty;
            }
            ILifetimeBorrower borrower = remoteObject as ILifetimeBorrower;
            if (borrower == null)
            {
                return Guid.Empty;
            }
            return borrower.RegisterSponsor(sponsor);
        }

        private void UninitializeSponsorship(T remoteObject, Guid cookie)
        {
            try
            {
                ILifetimeBorrower borrower = remoteObject as ILifetimeBorrower;
                if (borrower == null)
                {
                    return;
                }
                borrower.UnregisterSponsor(cookie);
            }
            catch (Exception)
            {
                
            }
        }

        //private static class ServerAliveKeeper
        //{
        //    private static readonly Timer Timer;
        //    private static readonly List<MarshalByRefObject> Objects;

        //    static ServerAliveKeeper()
        //    {
        //        Objects = new List<MarshalByRefObject>();
        //        Timer = new Timer(Constants.Remoting.ExpirationTime);
        //        Timer.Elapsed += OnAliveTimerElapsed;
        //        Timer.AutoReset = true;
        //        Timer.Start();
        //    }

        //    private static void OnAliveTimerElapsed(object sender, ElapsedEventArgs e)
        //    {
        //        lock (Objects)
        //        {
        //            foreach (MarshalByRefObject obj in Objects)
        //            {
        //                ILease lease = (ILease)RemotingServices.GetLifetimeService(obj);
        //                lease.Renew(TimeSpan.FromMilliseconds(Constants.Remoting.ExpirationTime + 5000));
        //            }
        //        }
        //    }

        //    public static void Register(MarshalByRefObject obj)
        //    {
        //        lock (Objects)
        //        {
        //            Objects.Add(obj);
        //        }
        //    }

        //    public static void Unregister(MarshalByRefObject obj)
        //    {
        //        lock (Objects)
        //        {
        //            Objects.Remove(obj);
        //        }
        //    }
        //}
    }
}
