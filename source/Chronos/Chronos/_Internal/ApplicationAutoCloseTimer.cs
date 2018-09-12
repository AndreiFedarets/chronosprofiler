using System;
using System.Timers;
using Chronos.Config;

namespace Chronos
{
    internal sealed class ApplicationAutoCloseTimer
    {
        private readonly ChronosApplication _application;
        private readonly Timer _timer;
        private volatile bool _autoCloseEnabled;

        public ApplicationAutoCloseTimer(ChronosApplication application, IConfigurationProvider configuration)
        {
            _application = application;
            _timer = new Timer();
            _timer.Elapsed += OnAutoCloseTimerElapsed;
            _timer.Interval = configuration.Daemon.AutoClose.Timeout;
            _autoCloseEnabled = configuration.Daemon.AutoClose.Enabled;
            _timer.Enabled = _autoCloseEnabled;
        }

        public bool Enabled { get; set; }

        public TimeSpan Interval { get; set; }

        public void Reset()
        {
            lock (_timer)
            {
                if (_timer.Enabled)
                {
                    _timer.Stop();
                    _timer.Start();
                }
            }
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Start()
        {
            if (_autoCloseEnabled && !_timer.Enabled)
            {
                _timer.Start();
            }
        }

        private void OnAutoCloseTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //_application.Close();
        }
    }
}
