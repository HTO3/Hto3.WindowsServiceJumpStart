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
    /// <summary>
    /// Service runner
    /// </summary>
    public static class Runner
    {
        private static ServiceConfiguration _serviceConfiguration;

        /// <summary>
        /// Open a dialog to manage the service if the application run in User interactive mode (Visual Studio run or execute as desktop application) or run as service.
        /// </summary>
        /// <param name="service">The service to run</param>
        public static void Run(ServiceBase service)
        {
            Runner.Run(new[] { service });
        }
        /// <summary>
        /// Open a dialog to manage the service if the application run in User interactive mode (Visual Studio run or execute as desktop application) or run as service.
        /// </summary>
        /// <param name="services">The services to run</param>
        public static void Run(ServiceBase[] services)
        {
            if (Environment.UserInteractive)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run
                    (
                        Runner.Configuration.RunAsApplication ?
                            (Form)new RunAsApplication() { Services = services } :
                            (Form)new Install() { Services = services }
                    );
            }
            else
            {
                ServiceBase.Run(services);
            }
        }
        /// <summary>
        /// Service configuration
        /// </summary>
        public static ServiceConfiguration Configuration
        {
            get { return _serviceConfiguration = _serviceConfiguration ?? new ServiceConfiguration(); }
        }
    }
}
