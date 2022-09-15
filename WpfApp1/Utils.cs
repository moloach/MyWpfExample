using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Utils
    {
        public static void KillSelfProcess()
        {
            Process thisProcess = Process.GetProcessById(Process.GetCurrentProcess().Id);
            thisProcess.Kill();
        }
    }
}
