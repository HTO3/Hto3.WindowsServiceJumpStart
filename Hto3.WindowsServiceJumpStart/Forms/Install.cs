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
    internal partial class Install : Form
    {
        public ServiceBase[] Services { get; set; }
        
        private AssemblyInstaller assemblyInstaller;
        private Boolean alreadyInstalled;        

        internal Install()
        {
            InitializeComponent();            
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

        private void Dialog_Load(object sender, EventArgs e)
        {
            if (this.Services == null || this.Services.Length == 0)
                throw new InvalidOperationException("There are no services to manage. Check the parameter in the Runner.Run() method.");

            this.lblName.Text = Runner.Configuration.DisplayName;
            this.lblDescription.Text = Runner.Configuration.Description;
            this.lblFrom.Text = Runner.Configuration.CompanyName;
            this.assemblyInstaller = Runner.Configuration.BuildAssemblyInstaller();
            this.RefreshInstallState();            
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
    }
}
