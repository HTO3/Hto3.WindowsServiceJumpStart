using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace Hto3.WindowsServiceJumpStart.Forms
{
    internal partial class RunAsApplication : Form
    {
        private MethodInfo onStartMethodInfo;
        private MethodInfo onContinueMethodInfo;
        private MethodInfo onPauseMethodInfo;
        private MethodInfo onStopMethodInfo;
        private MethodInfo initializeMethodInfo;
        private ServiceControllerStatus status;
        public ServiceBase[] Services { get; set; }

        public RunAsApplication()
        {
            InitializeComponent();
            this.onStartMethodInfo = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            this.onPauseMethodInfo = typeof(ServiceBase).GetMethod("OnPause", BindingFlags.Instance | BindingFlags.NonPublic);
            this.onContinueMethodInfo = typeof(ServiceBase).GetMethod("OnContinue", BindingFlags.Instance | BindingFlags.NonPublic);
            this.onStopMethodInfo = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            this.initializeMethodInfo = typeof(ServiceBase).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);
            this.status = ServiceControllerStatus.Stopped;
        }

        private void RunAsApplication_Load(object sender, EventArgs e)
        {
            if (this.Services == null || this.Services.Length == 0)
                throw new InvalidOperationException("There are no services to manage. Check the parameter in the Runner.Run() method.");

            this.Text = String.IsNullOrEmpty(Runner.Configuration.DisplayName) ? "Run" : Runner.Configuration.DisplayName;

            if (Runner.Configuration.StartType == ServiceStartMode.Automatic)
                this.BtnStart_Click(btnStart, new EventArgs());
            
            this.RefreshRunState();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            this.RefreshRunState();            
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Services.Length; i++)
                this.onStopMethodInfo.Invoke(this.Services[i], null);
            this.status = ServiceControllerStatus.Stopped;            
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            var multipleServices = this.Services.Length > 1;

            for (int i = 0; i < this.Services.Length; i++)
            {
                this.initializeMethodInfo.Invoke(this.Services[i], new Object[] { multipleServices });
                this.onStartMethodInfo.Invoke(this.Services[i], new Object[] { Environment.GetCommandLineArgs().Skip(1).ToArray() });
            }
            this.status = ServiceControllerStatus.Running;
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Services.Length; i++)
                this.onPauseMethodInfo.Invoke(this.Services[i], null);
            this.status = ServiceControllerStatus.Paused;
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Services.Length; i++)
                this.onContinueMethodInfo.Invoke(this.Services[i], null);
            this.status = ServiceControllerStatus.Running;
        }

        private void RefreshRunState()
        {
            switch (status)
            {
                case ServiceControllerStatus.Paused:
                    this.btnContinue.Enabled = true;
                    this.btnPause.Enabled = false;
                    this.btnStart.Enabled = false;
                    this.btnStop.Enabled = false;
                    break;
                case ServiceControllerStatus.Running:
                    this.btnContinue.Enabled = false;
                    this.btnPause.Enabled = true;
                    this.btnStart.Enabled = false;
                    this.btnStop.Enabled = true;
                    break;
                case ServiceControllerStatus.Stopped:
                    this.btnContinue.Enabled = false;
                    this.btnPause.Enabled = false;
                    this.btnStart.Enabled = true;
                    this.btnStop.Enabled = false;
                    break;
            }
        }        
    }
}
