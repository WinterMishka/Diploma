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
            _ = client.GetAsync("/api/test_notify");
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

        private async void SettingsChanged(object sender, EventArgs e)
        {
            var settings = new
            {
                notify_time = maskedTextBox1.Text,
                send_absent_only = guna2CheckBox1.Checked,
                send_daily_updates = guna2CheckBox2.Checked
            };

            var content = new StringContent(JsonConvert.SerializeObject(settings), Encoding.UTF8, "application/json");
            try { await client.PostAsync("/api/bot_settings", content); } catch { }
        }

        private async Task LoadSubscribers()
        {
            try
            {
                var resp = await client.GetAsync("/api/subscribers");
                if (!resp.IsSuccessStatusCode) return;
                var json = await resp.Content.ReadAsStringAsync();
                var subs = JsonConvert.DeserializeObject<List<Subscriber>>(json) ?? new List<Subscriber>();

                if (dataGridView1.Columns.Count == 0)
                {
                    dataGridView1.Columns.Add("Id", "Id");
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns.Add("No", "№");
                    dataGridView1.Columns.Add("FullName", "ФИО");
                    dataGridView1.Columns.Add("Role", "Роль");
                    dataGridView1.Columns.Add("TelegramID", "TelegramID");
                    dataGridView1.Columns.Add("Groups", "Группа(-ы)");
                    dataGridView1.Columns.Add("Status", "Статус");
                }

                dataGridView1.Rows.Clear();
                int i = 1;
                foreach (var s in subs)
                {
                    dataGridView1.Rows.Add(s.id, i++, s.full_name, s.role, s.telegram_id, s.groups, s.status ? "Подтверждён" : "Не подтверждён");
                }
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
