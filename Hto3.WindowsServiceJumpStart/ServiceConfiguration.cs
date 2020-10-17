using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace Hto3.WindowsServiceJumpStart
{
    /// <summary>
    /// Service configuration
    /// </summary>
    public class ServiceConfiguration
    {
        internal ServiceConfiguration()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var assemblyCompany = entryAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true).Cast<AssemblyCompanyAttribute>().FirstOrDefault();
            this.CompanyName = assemblyCompany?.Company;
        }
        /// <summary>
        /// Indicates the friendly name that identifies the service to the user.
        /// </summary>
        public String DisplayName { get; set; }
        /// <summary>
        /// Indicates the service's description (a brief comment that explains the purpose of the service).
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// Indicates the services that must be running in order for this service to run.
        /// </summary>
        public String[] ServicesDependedOn { get; set; }
        /// <summary>
        /// Indicates the name used by the system to identify this service.
        /// </summary>
        public String ServiceName { get; set; }
        /// <summary>
        /// Indicates how and when this service is started.
        /// </summary>
        public ServiceStartMode StartType { get; set; }
        /// <summary>
        /// Contains the dalayed auto-start setting of service. This setting is ignored unless the service is an auto-start service.
        /// </summary>
        public Boolean DelayedAutoStart { get; set; }
        /// <summary>
        /// Indicates the account type under which the service will run.
        /// </summary>
        public ServiceAccount Account { get; set; }
        /// <summary>
        /// Indicates who is the company that build this service. By default, this property will have the company name from the entry assembly.
        /// </summary>
        public String CompanyName { get; set; }
        /// <summary>
        /// If true, a dialog will show with the controls responsible to run the service as a desktop application.
        /// </summary>
        public Boolean RunAsApplication { get; set; }
        /// <summary>
        /// If true, the dialog will keep open after service install or uninstall.
        /// </summary>
        public Boolean KeepDialogOpenWhenDone { get; set; }

        internal AssemblyInstaller BuildAssemblyInstaller()
        {
            var srvInstaller = new ServiceInstaller();
            srvInstaller.DisplayName = this.DisplayName;
            srvInstaller.ServiceName = this.ServiceName;
            srvInstaller.Description = this.Description;
            srvInstaller.ServicesDependedOn = this.ServicesDependedOn;
            srvInstaller.StartType = this.StartType;
            srvInstaller.DelayedAutoStart = this.DelayedAutoStart;

            var srvProcessInstaller = new ServiceProcessInstaller();
            srvProcessInstaller.Account = this.Account;

            var srvAssembyInstaller = new AssemblyInstaller(Assembly.GetEntryAssembly(), null);

            srvAssembyInstaller.Installers.Add(srvInstaller);
            srvAssembyInstaller.Installers.Add(srvProcessInstaller);

            return srvAssembyInstaller;
        }
    }
}
