using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Diploma.Classes;

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
            CheckBoxUI.ApplyRecursive(this);
            Load += TelegramBotControl_Load;
            guna2CheckBox1.CheckedChanged += SettingsChanged;
            guna2CheckBox2.CheckedChanged += SettingsChanged;
            maskedTextBox1.Leave += SettingsChanged;

            this.Load += (s, e) =>
            {
                if (FindForm() is FaceControl face)
                    UiSettingsManager.ApplyTo(face);
            };

            // save options when the control is closed so the latest values
            // persist across application restarts
            HandleDestroyed += (s, e) => SaveLocalSettings();
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
            LoadLocalSettings();

            // Отключаем обработчики
            guna2CheckBox1.CheckedChanged -= SettingsChanged;
            guna2CheckBox2.CheckedChanged -= SettingsChanged;

            await LoadSettings();

            // Включаем обратно после загрузки настроек с сервера
            guna2CheckBox1.CheckedChanged += SettingsChanged;
            guna2CheckBox2.CheckedChanged += SettingsChanged;

            await LoadSubscribers();
        }

        private void LoadLocalSettings()
        {
            Properties.Settings.Default.Reload();

            // Отключаем обработчики перед массовым применением локальных настроек
            guna2CheckBox1.CheckedChanged -= SettingsChanged;
            guna2CheckBox2.CheckedChanged -= SettingsChanged;

            maskedTextBox1.Text = Properties.Settings.Default.TelegramNotifyTime;
            guna2CheckBox1.Checked = Properties.Settings.Default.TelegramSendAbsentOnly;
            guna2CheckBox2.Checked = Properties.Settings.Default.TelegramSendDailyUpdates;

            guna2CheckBox1.Refresh();
            guna2CheckBox2.Refresh();

            // Включаем обратно
            guna2CheckBox1.CheckedChanged += SettingsChanged;
            guna2CheckBox2.CheckedChanged += SettingsChanged;
        }

        private async Task LoadSettings()
        {
            try
            {
                var resp = await client.GetAsync("/api/bot_settings");
                if (!resp.IsSuccessStatusCode) return;
                var json = await resp.Content.ReadAsStringAsync();
                dynamic cfg = JsonConvert.DeserializeObject(json);

                // Отключаем обработчики чтобы не триггерить SettingsChanged
                guna2CheckBox1.CheckedChanged -= SettingsChanged;
                guna2CheckBox2.CheckedChanged -= SettingsChanged;

                maskedTextBox1.Text = (string)cfg.notify_time;
                guna2CheckBox1.Checked = cfg.send_absent_only;
                guna2CheckBox2.Checked = cfg.send_daily_updates;

                guna2CheckBox1.Refresh();
                guna2CheckBox2.Refresh();

                // Включаем обратно
                guna2CheckBox1.CheckedChanged += SettingsChanged;
                guna2CheckBox2.CheckedChanged += SettingsChanged;

                SaveLocalSettings();
            }
            catch { }
        }


        private void SaveLocalSettings()
        {
            Properties.Settings.Default.TelegramNotifyTime = maskedTextBox1.Text;
            Properties.Settings.Default.TelegramSendAbsentOnly = guna2CheckBox1.Checked;
            Properties.Settings.Default.TelegramSendDailyUpdates = guna2CheckBox2.Checked;
            Properties.Settings.Default.Save();
        }

        private async void SettingsChanged(object sender, EventArgs e)
        {
            var settings = new
            {
                notify_time = maskedTextBox1.Text,
                send_absent_only = guna2CheckBox1.Checked,
                send_daily_updates = guna2CheckBox2.Checked
            };
            SaveLocalSettings();
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
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridView1.MultiSelect = false;
                }

                dataGridView1.Rows.Clear();
                int i = 1;
                foreach (var s in subs)
                {
                    dataGridView1.Rows.Add(s.id, i++, s.full_name, s.role, s.telegram_id, s.groups, s.status ? "Подтверждён" : "Не подтверждён");
                }
                Classes.DataGridViewUI.BeautifyGrid(dataGridView1);
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
            if (dataGridView1.SelectedRows.Count == 0) return;

            var confirm = MessageBox.Show(
                "Удалить выбранного подписчика?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                int id = Convert.ToInt32(row.Cells["Id"].Value);
                try { await client.PostAsync($"/api/delete_subscriber/{id}", null); } catch { }
            }
            await LoadSubscribers();
        }
    }
}
