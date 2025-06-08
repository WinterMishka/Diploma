using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diploma.Classes;
using System.Diagnostics;
using System.IO;

namespace Diploma
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            StartServer();
            if (!WaitForServer())
            {
                MessageBox.Show("Сервер не доступен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UiSettingsManager.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new FaceControl();
            UiSettingsManager.ApplyTo(form);
            if (UiSettingsManager.Current.StartFullScreen)
                form.WindowState = FormWindowState.Maximized;
            Application.Run(form);
        }

        #region Методы

        private static void StartServer()
        {
            if (PingServer())
                return;

            try
            {
                var exeDir = Application.StartupPath;
                var exePath = Path.Combine(exeDir, "Server", "dist", "server.exe");
                if (File.Exists(exePath))
                {
                    Process.Start(new ProcessStartInfo(exePath)
                    {
                        WorkingDirectory = Path.GetDirectoryName(exePath),
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                }
                else
                {
                    var pyPath = Path.Combine(exeDir, "Server", "server.py");
                    if (File.Exists(pyPath))
                    {
                        Process.Start(new ProcessStartInfo("python", pyPath)
                        {
                            WorkingDirectory = Path.GetDirectoryName(pyPath),
                            UseShellExecute = false,
                            CreateNoWindow = true
                        });
                    }
                }
            }
            catch { }
        }

        private static bool WaitForServer(int attempts = 30, int delayMs = 1000)
        {
            for (int i = 0; i < attempts; i++)
            {
                if (PingServer())
                    return true;
                Task.Delay(delayMs).Wait();
            }
            return false;
        }

        private static bool PingServer()
        {
            try
            {
                using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
                {
                    var resp = client.GetAsync("http://127.0.0.1:5000/ping").Result;
                    return resp.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
