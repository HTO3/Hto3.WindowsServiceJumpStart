using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Hto3.WindowsServiceJumpStart;

namespace Hto3.WindowsServiceJumpStart.Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceTest()
            };

            //Old way
            //ServiceBase.Run(ServicesToRun);

            Runner.Configuration.Description = "This is a dummy service create by test purposes.";
            Runner.Configuration.DisplayName = "HTO3 dummy service";
            Runner.Configuration.ServiceName = "HTO3Dummy";
            Runner.Configuration.StartType = ServiceStartMode.Automatic;
            Runner.Configuration.AllowRunningAsApplication = true;

            Runner.Run(ServicesToRun);
        }
    }
}
