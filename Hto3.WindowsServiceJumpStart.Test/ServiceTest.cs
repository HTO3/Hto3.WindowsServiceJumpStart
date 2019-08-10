using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.WindowsServiceJumpStart.Test
{
    public partial class ServiceTest : ServiceBase
    {
        public ServiceTest()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Debug.WriteLine("Service started.");
        }

        protected override void OnStop()
        {
            Debug.WriteLine("Service stopped.");
        }

        protected override void OnContinue()
        {
            Debug.WriteLine("Service resumed.");
        }

        protected override void OnPause()
        {
            Debug.WriteLine("Service paused.");
        }
    }
}
