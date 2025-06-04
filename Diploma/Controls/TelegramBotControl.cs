using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Diploma
{
    public partial class TelegramBotControl : UserControl
    {
        private readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5000") };

        private class Subscriber
        {
            public int id { get; set; }
            public string full_name { get; set; }
            public string role { get; set; }
            public long telegram_id { get; set; }
            public string groups { get; set; }
            public bool status { get; set; }
        }

        public TelegramBotControl()
        {
            InitializeComponent();
            Load += TelegramBotControl_Load;
            guna2CheckBox1.CheckedChanged += SettingsChanged;
            guna2CheckBox2.CheckedChanged += SettingsChanged;
            maskedTextBox1.Leave += SettingsChanged;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ConfirmSelected();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            _ = LoadSubscribers();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                var tg = row.Cells["TelegramID"].Value?.ToString();
                if (!string.IsNullOrEmpty(tg))
                {
                    _ = client.GetAsync($"/api/test_notify/{tg}");
                }
            }
        }

        private async void TelegramBotControl_Load(object sender, EventArgs e)
        {
            await LoadSettings();
            await LoadSubscribers();
        }

        private async Task LoadSettings()
        {
            try
            {
                var resp = await client.GetAsync("/api/bot_settings");
                if (!resp.IsSuccessStatusCode) return;
                var json = await resp.Content.ReadAsStringAsync();
                dynamic cfg = JsonConvert.DeserializeObject(json);
                maskedTextBox1.Text = (string)cfg.notify_time;
                guna2CheckBox1.Checked = cfg.send_absent_only;
                guna2CheckBox2.Checked = cfg.send_daily_updates;
            }
            catch { }
        }

        private async void ConfirmSelected()
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                int id = Convert.ToInt32(row.Cells["Id"].Value);
                try { await client.PostAsync($"/api/confirm_subscriber/{id}", null); } catch { }
            }
            await LoadSubscribers();
        }

        private async void DeleteSelected()
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                int id = Convert.ToInt32(row.Cells["Id"].Value);
                try { await client.PostAsync($"/api/delete_subscriber/{id}", null); } catch { }
            }
            await LoadSubscribers();
        }
    }
}
