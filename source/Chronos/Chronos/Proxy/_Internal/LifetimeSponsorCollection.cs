using System;
using System.Collections.Generic;
using System.Timers;

namespace Chronos.Proxy
{
    internal sealed class LifetimeSponsorCollection : ILifetimeBorrower
    {
        private readonly IDictionary<Guid, ILifetimeSponsor> _sponsors;
        private readonly TimeSpan _sponsorshipTimeout;
        private readonly Timer _timer;

        public LifetimeSponsorCollection(TimeSpan sponsorshipTimeout)
        {
            _sponsors = new Dictionary<Guid, ILifetimeSponsor>();
            _sponsorshipTimeout = sponsorshipTimeout;
            _timer = new Timer();
            _timer.Elapsed += OnTimerElapsed;
            _timer.Interval = sponsorshipTimeout.TotalMilliseconds;
            _timer.AutoReset = true;
        }

        public event EventHandler SponsorshipEnded;

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public Guid RegisterSponsor(ILifetimeSponsor sponsor)
        {
            Guid cookie = Guid.NewGuid();
            lock (_sponsors)
            {
                _sponsors[cookie] = sponsor;
            }
            return cookie;
        }

        public void UnregisterSponsor(Guid cookie)
        {
            lock (_sponsors)
            {
                _sponsors.Remove(cookie);
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!ShouldStayAlive())
            {
                SponsorshipEnded.SafeInvoke(this, EventArgs.Empty);
            }
        }

        private bool GetSponsorshipApproval(ILifetimeSponsor sponsor, Guid cookie, ref List<Guid> diedSponsors)
        {
            const int attemptsCount = 3;
            const int attemptsDelay = 1000;
            for (int i = 1; i <= attemptsCount; i++)
            {
                try
                {
                    return sponsor.ShouldStayAlive();
                }
                catch (Exception)
                {
                    if (i <= attemptsCount)
                    {
                        System.Threading.Thread.Sleep(attemptsDelay);
                    }
                    else
                    {
                        diedSponsors = diedSponsors ?? new List<Guid>();
                        diedSponsors.Add(cookie);
                        return false;
                    }
                }
            }
            return false;
        }

        public bool ShouldStayAlive()
        {
            IDictionary<Guid, ILifetimeSponsor> collection;
            lock (_sponsors)
            {
                collection = new Dictionary<Guid, ILifetimeSponsor>(_sponsors);
            }
            List<Guid> diedSponsors = null;
            bool sponsorshipApproved = false;
            foreach (KeyValuePair<Guid, ILifetimeSponsor> pair in collection)
            {
                bool result = GetSponsorshipApproval(pair.Value, pair.Key, ref diedSponsors);
                if (result)
                {
                    sponsorshipApproved = true;
                    break;
                }
            }
            if (diedSponsors != null)
            {
                lock (_sponsors)
                {
                    foreach (Guid diedSponsor in diedSponsors)
                    {
                        _sponsors.Remove(diedSponsor);
                    }
                }
            }
            return sponsorshipApproved;
        }
    }
}
