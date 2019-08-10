using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace Hto3.WindowsServiceJumpStart
{
    public class ServiceConfiguration
    {
        internal ServiceConfiguration()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var assemblyCompany = entryAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true).Cast<AssemblyCompanyAttribute>().FirstOrDefault();
            this.CompanyName = assemblyCompany?.Company;
        }
        public String DisplayName { get; set; }
        public String Description { get; set; }
        public String[] ServicesDependedOn { get; set; }
        public String ServiceName { get; set; }
        public ServiceStartMode StartType { get; set; }
        public Boolean DelayedAutoStart { get; set; }
        public ServiceAccount Account { get; set; }
        public String CompanyName { get; set; }
        public Boolean AllowRunningAsApplication { get; set; }
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
