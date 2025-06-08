using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Diploma.Classes
{
    internal static class ServerProcessManager
    {
        private static Process _process;
        public static event Action<string> OutputReceived;

        public static void Start()
        {
            if (PingServer() || IsProcessRunning())
                return;

            string distDir = Path.Combine(AppPaths.ServerRoot, "dist");
            string exePath = Path.Combine(distDir, "server.exe");
            string pyPath = Path.Combine(distDir, "server.py");

            string file;
            string args = string.Empty;
            if (File.Exists(exePath))
            {
                file = exePath;
            }
            else if (File.Exists(pyPath))
            {
                file = "python";
                args = pyPath;
            }
            else
            {
                return;
            }

            var psi = new ProcessStartInfo(file, args)
            {
                WorkingDirectory = distDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            try
            {
                _process = Process.Start(psi);
                if (_process != null)
                {
                    _process.OutputDataReceived += (s, e) => { if (e.Data != null) OutputReceived?.Invoke(e.Data); };
                    _process.ErrorDataReceived += (s, e) => { if (e.Data != null) OutputReceived?.Invoke(e.Data); };
                    _process.BeginOutputReadLine();
                    _process.BeginErrorReadLine();
                }
            }
            catch { }
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
                    _process.Kill(true);
                    _process.WaitForExit();
                }
            }
            catch { }
            _process = null;
        }

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

                        if (file.EndsWith("server.exe") || file.EndsWith("server.py") || name == "server")
                            return true;
                    }
                    catch { }
                }
            }
            catch { }
            return false;
        }
    }
}
