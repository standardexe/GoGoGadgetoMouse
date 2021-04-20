using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    static class Program {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            WinAPI.SetProcessDpiAwareness(WinAPI.DpiAwareness.PerMonitorAware);

            Application.Run(new ApplicationContext());
        }
    }

}
