using Hto3.WindowsServiceJumpStart.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hto3.WindowsServiceJumpStart
{
    public static class Runner
    {
        private static ServiceConfiguration _serviceConfiguration;

        public static void Run(ServiceBase service)
        {
            Runner.Run(new[] { service });
        }

        public static void Run(ServiceBase[] services)
        {
            if (Environment.UserInteractive)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new Dialog() { Services = services });
            }
            else
            {
                ServiceBase.Run(services);
            }
        }

        public static ServiceConfiguration Configuration
        {
            get { return _serviceConfiguration = _serviceConfiguration ?? new ServiceConfiguration(); }
        }
    }
}
