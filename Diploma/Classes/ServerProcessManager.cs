#region Используемые пространства имён
using System;
using System.Diagnostics;
using System.IO;
using Diploma.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

namespace Diploma.Classes
{
    internal static class ServerProcessManager
    {
        #region Поля
        private static Process _process;
        private static string _lastOutput = string.Empty;
        private static readonly System.Collections.Generic.List<string> _logLines = new System.Collections.Generic.List<string>();
        public static event Action<string> OutputReceived;
        public static string LastOutput => _lastOutput;
        public static System.Collections.Generic.IReadOnlyList<string> LogLines => _logLines.AsReadOnly();
        #endregion

        #region API
        public static void Start()
        {
            if (PingServer() || IsProcessRunning())
                return;

            string distDir = AppPaths.ServerDist;
            string exePath = Path.Combine(distDir, "server.exe");

            if (!File.Exists(exePath))
            {
                System.Windows.Forms.MessageBox.Show("server.exe не найден: " + exePath);
                Application.Exit();
                return;
            }

            var psi = new ProcessStartInfo(exePath)
            {
                WorkingDirectory = distDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                _lastOutput = string.Empty;
                _logLines.Clear();
                _process = Process.Start(psi);
                if (_process != null)
                {
                    _process.OutputDataReceived += (s, e) =>
                    {
                        if (e.Data != null)
                        {
                            _logLines.Add(e.Data);
                            _lastOutput = e.Data;
                            OutputReceived?.Invoke(e.Data);
                        }
                    };
                    _process.ErrorDataReceived += (s, e) =>
                    {
                        if (e.Data != null)
                        {
                            _logLines.Add(e.Data);
                            _lastOutput = e.Data;
                            OutputReceived?.Invoke(e.Data);
                        }
                    };
                    _process.BeginOutputReadLine();
                    _process.BeginErrorReadLine();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка запуска сервера: " + ex);
            }
        }

        public static async void Restart()
        {
            Stop();
            await Task.Delay(500);
            Start();
        }

        public static void Stop()
        {
            try
            {
                if (_process != null && !_process.HasExited)
                {
                    _process.Kill();
                    _process.WaitForExit(2000);
                }
            }
            catch { }
            _process = null;
            KillAllServerProcesses();
        }

        #endregion

        #region Вспомогательные методы
        private static bool PingServer()
        {
            try
            {
                using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(2) })
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

        private static bool IsProcessRunning()
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

                        if (file.EndsWith("server.exe") || name == "server")
                            return true;
                    }
                    catch { }
                }
            }
            catch { }
            return false;
        }

        private static void KillAllServerProcesses()
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

                        if (file.EndsWith("server.exe") || name == "server")
                        {
                            if (!proc.HasExited)
                            {
                                proc.Kill();
                                proc.WaitForExit(2000);
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
        #endregion
    }
}
