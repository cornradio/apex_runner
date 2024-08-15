using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apex_runner
{
    internal static class Program
    {

        // 导入 Win32 API 用于设置前台窗口
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [STAThread]
        static void Main()
        {
            // 使用互斥体确保只有一个实例运行
            bool isNewInstance;
            using (Mutex mutex = new Mutex(true, "MyUniqueAppName", out isNewInstance))
            {
                if (isNewInstance)
                {
                    /// <summary>
                    /// 应用程序的主入口点。
                    /// </summary>
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
                else
                {
                    // 找到已经运行的实例，并将其激活到前台
                    Process currentProcess = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
                    {
                        if (process.Id != currentProcess.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }

        }
    }
}