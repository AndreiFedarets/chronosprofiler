using System;
using System.Diagnostics;
using Chronos.Core;
using Rhiannon.Extensions;

namespace Chronos.Daemon.Internal
{
	internal class AgentProcessManager
	{
		private readonly Process _agentProcess;
		private readonly ProcessInfo _processInfo;

        public AgentProcessManager(int processId, uint syncTime)
		{
			_agentProcess = Process.GetProcessById(processId);
			_agentProcess.Exited += OnAgentProcessExited;
			_agentProcess.EnableRaisingEvents = true;
			byte[] icon = _agentProcess.GetIconBytes();
			string executableFullName = _agentProcess.GetExecutableFullName();
            _processInfo = new ProcessInfo(_agentProcess.Id, _agentProcess.ProcessName, executableFullName, icon, _agentProcess.StartTime, syncTime);
		}

		private void OnAgentProcessExited(object sender, EventArgs e)
		{
			ProcessShutdown.SafeInvoke();
		}

		public ProcessInfo ProcessInfo
		{
			get { return _processInfo; }
		}

		public event Action ProcessShutdown;

		public void Close()
		{
			_agentProcess.CloseMainWindow();
			_agentProcess.WaitForExit();
		}

		public void Dispose()
		{
			_agentProcess.Exited -= OnAgentProcessExited;
			_agentProcess.Dispose();
		}
	}
}
