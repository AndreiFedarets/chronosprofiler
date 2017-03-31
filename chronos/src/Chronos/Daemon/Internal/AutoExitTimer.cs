using System;
using System.Timers;
using Chronos.Core;

namespace Chronos.Daemon.Internal
{
	internal class AutoExitTimer
	{
		private readonly Timer _timer;
		private readonly Action _closeApplicationAction;
		private SessionState _state;

		public AutoExitTimer(Action closeApplicationAction, int autoExitTime)
		{
			_closeApplicationAction = closeApplicationAction;
			_timer = new Timer();
			_timer.Elapsed += OnTimerElapsed;
			_timer.Interval = autoExitTime*1000;
			_timer.Enabled = IsEnabled;
		}

		private void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			_closeApplicationAction();
		}

		public bool IsEnabled
		{
			get { return State == SessionState.Closed || State == SessionState.Decoding; }
		}

		public SessionState State
		{
			get { return _state; }
			set
			{
				_state = value;
				_timer.Enabled = IsEnabled;
			}
		}

		public void Refresh()
		{
			if (IsEnabled)
			{
				bool state = _timer.Enabled;
				_timer.Enabled = !state;
				_timer.Enabled = state;
			}
		}

		public void Dispose()
		{
			_timer.Elapsed -= OnTimerElapsed;
			_timer.Dispose();
		}
	}
}
