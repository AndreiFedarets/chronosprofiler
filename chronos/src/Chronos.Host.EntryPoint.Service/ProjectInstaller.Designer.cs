namespace Chronos.Host.EntryPoint.Service
{
	partial class ProjectInstaller
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
			this._serviceInstaller = new System.ServiceProcess.ServiceInstaller();
			// 
			// _serviceProcessInstaller
			// 
			this._serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this._serviceProcessInstaller.Password = null;
			this._serviceProcessInstaller.Username = null;
			// 
			// _serviceInstaller
			// 
			this._serviceInstaller.ServiceName = "Chronos Host Service";
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this._serviceProcessInstaller,
            this._serviceInstaller});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller _serviceProcessInstaller;
		private System.ServiceProcess.ServiceInstaller _serviceInstaller;
	}
}