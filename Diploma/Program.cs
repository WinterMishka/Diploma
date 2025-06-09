using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diploma.Classes;
using System.Diagnostics;

namespace Diploma
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ServerProcessManager.Start();
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
            ServerProcessManager.Stop();
        }

        #region Методы

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
