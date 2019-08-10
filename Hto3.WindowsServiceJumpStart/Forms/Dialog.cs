using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace Hto3.WindowsServiceJumpStart.Forms
{
    public partial class Dialog : Form
    {
        public ServiceBase[] Services { get; set; }
        private MethodInfo onStartMethodInfo;
        private MethodInfo onContinueMethodInfo;
        private MethodInfo onPauseMethodInfo;
        private MethodInfo onStopMethodInfo;
        private MethodInfo initializeMethodInfo;
        private AssemblyInstaller assemblyInstaller;
        private Boolean alreadyInstalled;
        private ServiceControllerStatus status;

        public Dialog()
        {
            InitializeComponent();
            this.onStartMethodInfo = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            this.onPauseMethodInfo = typeof(ServiceBase).GetMethod("OnPause", BindingFlags.Instance | BindingFlags.NonPublic);
            this.onContinueMethodInfo = typeof(ServiceBase).GetMethod("OnContinue", BindingFlags.Instance | BindingFlags.NonPublic);
            this.onStopMethodInfo = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            this.initializeMethodInfo = typeof(ServiceBase).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);
            this.status = ServiceControllerStatus.Stopped;
        }

        private void BtnDontInstall_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnInstall_Click(object sender, EventArgs e)
        {
            var savedState = new Hashtable();

            try
            {
                if (this.alreadyInstalled)
                {
                    this.assemblyInstaller.Uninstall(savedState);
                    MessageBox.Show("Successfully uninstalled", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    this.assemblyInstaller.Install(savedState);
                    this.assemblyInstaller.Commit(savedState);
                    MessageBox.Show("Successfully installed", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to install. Original error message:\r\n\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                try
                {
                    assemblyInstaller.Rollback(savedState);
                }
                catch { }
            }
            finally
            {
                if (!Runner.Configuration.KeepDialogOpenWhenDone)
                    Application.Exit();
                else
                    this.RefreshInstallState();
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Services.Length; i++)
                this.onStopMethodInfo.Invoke(this.Services[i], null);
            this.status = ServiceControllerStatus.Stopped;
            this.RefreshRunState();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            var multipleServices = this.Services.Length > 1;

            for (int i = 0; i < this.Services.Length; i++)
            {
                this.initializeMethodInfo.Invoke(this.Services[i], new Object[] { multipleServices });
                this.onStartMethodInfo.Invoke(this.Services[i], new Object[] { null });
            }
            this.status = ServiceControllerStatus.Running;
            this.RefreshRunState();
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Services.Length; i++)
                this.onPauseMethodInfo.Invoke(this.Services[i], null);
            this.status = ServiceControllerStatus.Paused;
            this.RefreshRunState();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Services.Length; i++)
                this.onContinueMethodInfo.Invoke(this.Services[i], null);
            this.status = ServiceControllerStatus.Running;
            this.RefreshRunState();
        }

        private void Dialog_Load(object sender, EventArgs e)
        {
            if (this.Services == null || this.Services.Length == 0)
                throw new InvalidOperationException("There are no services to manage.");
            this.gbxRunningAsApplication.Visible = Runner.Configuration.AllowRunningAsApplication;
            this.lblName.Text = Runner.Configuration.DisplayName;
            this.lblDescription.Text = Runner.Configuration.Description;
            this.lblFrom.Text = Runner.Configuration.CompanyName;
            this.assemblyInstaller = Runner.Configuration.BuildAssemblyInstaller();
            this.RefreshInstallState();
            this.RefreshRunState();
        }

        private void RefreshInstallState()
        {
            this.alreadyInstalled = ServiceController.GetServices().Any(s => s.ServiceName == Runner.Configuration.ServiceName);
            if (this.alreadyInstalled)
            {
                btnDontInstall.Text = "Exit";
                btnInstall.Text = "Uninstall";
                this.Text = "Service Uninstall";
                lblTitle.Text = "Do you want to uninstall this service?";
            }
            else
            {
                btnDontInstall.Text = "Don't Install";
                btnInstall.Text = "Install";
                this.Text = "Service Install";
                lblTitle.Text = "Do you want to install this service?";
            }
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
