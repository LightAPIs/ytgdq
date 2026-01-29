using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ListTrayBarInfo;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WindowsFormsApplication2
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!System.IO.File.Exists(Application.StartupPath + "\\TyDll.dll"))
            {
                MessageBox.Show("未找到TyDll.dll文件！");
                Application.Exit();
            }
            bool is_createdNew2;
            System.Threading.Mutex mu2 = null;
            try
            {
                #region 檢查程式是否重複執行

                Process cur = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName(cur.ProcessName);
                foreach (var pro in processes)
                {
                    if (pro.Id != cur.Id)
                    {
                        if (pro.MainModule.FileName == cur.MainModule.FileName)
                        {
                            MessageBox.Show("雨天跟打器对你弱弱地说：\"我已经打开了啦~~！\"", "雨天跟打器", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                // 在完全相同的傳入參數下不允許重複執行，避免數據重複計算
                string mutexName2 = "Args_" + String.Join("_", args).Replace(System.IO.Path.DirectorySeparatorChar, '_');
                mu2 = new System.Threading.Mutex(true, "Global\\" + mutexName2, out is_createdNew2);
                if (!is_createdNew2)
                {
                    MessageBox.Show("程序已运行！");
                    return;
                }

                #endregion

                //DoSomething();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Enable High DPI support for clear text rendering
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}
