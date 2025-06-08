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
            if (PingServer() || IsServerProcessRunning())
                return;

            try
            {
                var exeDir = Application.StartupPath;
                var rootDir = Path.GetFullPath(Path.Combine(exeDir, "..", ".."));

                string[] candidates =
                {
                    Path.Combine(exeDir, "Server", "dist", "server.exe"),
                    Path.Combine(rootDir, "Server", "dist", "server.exe"),
                    Path.Combine(exeDir, "Server", "server.py"),
                    Path.Combine(rootDir, "Server", "server.py")
                };

                foreach (var path in candidates)
                {
                    if (!File.Exists(path))
                        continue;

                    if (path.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        Process.Start(new ProcessStartInfo(path)
                        {
                            WorkingDirectory = Path.GetDirectoryName(path),
                            UseShellExecute = false,
                            CreateNoWindow = true
                        });
                    }
                    else if (path.EndsWith(".py", StringComparison.OrdinalIgnoreCase))
                    {
                        Process.Start(new ProcessStartInfo("python", path)
                        {
                            WorkingDirectory = Path.GetDirectoryName(path),
                            UseShellExecute = false,
                            CreateNoWindow = true
                        });
                    }
                    break;
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

        private static bool IsServerProcessRunning()
        {
            try
            {
                foreach (var proc in Process.GetProcesses())
                {
                    try
                    {
                        var name = proc.ProcessName.ToLowerInvariant();
                        if (!name.Contains("server"))
                            continue;

                        string file = string.Empty;
                        try { file = proc.MainModule.FileName.ToLowerInvariant(); } catch { }

                        if (file.EndsWith("server.exe") || file.EndsWith("server.py") || name == "server")
                            return true;
                    }
                    catch { }
                }
            }
            catch { }
            return false;
        }
        #endregion
    }
}
