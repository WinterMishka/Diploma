#region using
using Diploma.Data;
using Diploma.Helpers;
using Diploma.Recognition;
using Diploma.Services;
using Diploma.Classes;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace Diploma
{
    public partial class EnableControl : UserControl
    {
        #region Сервисы
        private readonly CameraService _cam = CameraService.Instance;
        private RecognitionService _recog;
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:5000") };
        private readonly IAppDbService _db;
        private Bitmap latestFrame;
        #endregion

        #region Конструкторы
        public EnableControl()
            : this(new SqlAppDbService(
                   Properties.Settings.Default.EducationAccessSystemConnectionString))
        {
        }

        public EnableControl(IAppDbService db)
        {
            InitializeComponent();
            CheckBoxUI.ApplyRecursive(this);

            _db = db;
            LoadVisitLog();

            _recog = new RecognitionService(_cam);
            _recog.Recognized += OnRecognized;

            guna2CheckBox1.CheckedChanged += CheckBoxChanged;
            guna2CheckBox2.CheckedChanged += CheckBoxChanged;

            CheckBoxChanged(this, EventArgs.Empty);

            this.Load += (s, e) =>
            {
                if (FindForm() is FaceControl face)
                    UiSettingsManager.ApplyTo(face);
            };

            Disposed += (s, e) => DisposeRecognition();
        }
        #endregion

        #region Запуск / остановка распознавания
        private void guna2BtnStartRecognition_Click(object sender, EventArgs e)
        {
            if (_recog == null)
            {
                _recog = new RecognitionService(_cam);
                _recog.Recognized += OnRecognized;
            }

            _cam.Subscribe(OnFrameArrived);
            _recog.Start();
            guna2CheckBox1.Enabled = guna2CheckBox2.Enabled = false;
        }

        private void guna2BtnStopRecognition_Click(object sender, EventArgs e)
        {
            _recog?.Stop();
            _recog?.Dispose();
            _recog = null;

            _cam.Unsubscribe(OnFrameArrived);
            latestFrame?.Dispose();
            latestFrame = null;

            guna2CheckBox1.Enabled = guna2CheckBox2.Enabled = true;
            guna2PictureBoxLiveCamera.Image = null;
            guna2PanelLastCapturedFace.BackgroundImage = null;
            guna2PanelSelectedFace.BackgroundImage = null;
        }
        #endregion

        #region Поток кадров
        private void OnFrameArrived(object sender, Bitmap frame)
        {
            latestFrame?.Dispose();
            latestFrame = (Bitmap)frame.Clone();

            if (guna2PictureBoxLiveCamera.IsHandleCreated &&
                !guna2PictureBoxLiveCamera.IsDisposed &&
                !guna2PictureBoxLiveCamera.Disposing)
            {
                var copy = (Bitmap)frame.Clone();
                try
                {
                    guna2PictureBoxLiveCamera.BeginInvoke((MethodInvoker)(() =>
                    {
                        if (guna2PictureBoxLiveCamera.IsDisposed) { copy.Dispose(); return; }
                        guna2PictureBoxLiveCamera.Image?.Dispose();
                        guna2PictureBoxLiveCamera.Image = copy;
                    }));
                }
                catch (InvalidAsynchronousStateException)
                {
                    copy.Dispose();
                }
            }
        }
        #endregion

        #region Обработчик распознавания
        private void OnRecognized(object s, RecognitionResult res)
        {
            if (res.id == "unknown" || !int.TryParse(res.id, out int id)) return;

            bool isDeparture = guna2CheckBox2.Checked;

            BeginInvoke(new MethodInvoker(async () =>
            {
                _db.SaveVisit(id, !isDeparture);
                ShowFaceFromBase64(res.face_image);
                SaveDetectedFaceToDisk(id, res.face_image);

                LoadVisitLog();
                SelectRowById(id);

                var info = _db.GetPersonInfo(id);

                try
                {
                    var payload = JsonConvert.SerializeObject(new
                    {
                        full_name = info.FullName,
                        status = isDeparture ? "Уход" : "Приход"
                    });
                    var content = new StringContent(payload, Encoding.UTF8, "application/json");
                    await _http.PostAsync("/api/notify_visit", content);
                }
                catch { }

                MessageBox.Show(
                    $"{info.FullName}\n{info.Status}\n{(isDeparture ? "Уход" : "Приход")}",
                    "Распознано",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }));
        }
        #endregion

        #region Загрузка и выбор строки
        private void LoadVisitLog()
        {
            BindLog(_db.GetVisitLog());
        }
        #endregion

        #region Вспомогательная привязка грида журнала
        private void BindLog(DataTable tbl)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = tbl;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dataGridView1.Columns.Contains("ID"))
                dataGridView1.Columns["ID"].HeaderText = "ID";
            if (dataGridView1.Columns.Contains("ФИО"))
                dataGridView1.Columns["ФИО"].HeaderText = "ФИО";
            if (dataGridView1.Columns.Contains("Статус"))
                dataGridView1.Columns["Статус"].HeaderText = "Статус";
            if (dataGridView1.Columns.Contains("Приход/Уход"))
                dataGridView1.Columns["Приход/Уход"].HeaderText = "Приход/Уход";
            if (dataGridView1.Columns.Contains("Дата"))
                dataGridView1.Columns["Дата"].HeaderText = "Дата";
            if (dataGridView1.Columns.Contains("Время"))
                dataGridView1.Columns["Время"].HeaderText = "Время";

            DataGridViewUI.BeautifyGrid(dataGridView1);
        }

        private void SelectRowById(int photoId)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                object v = row.Cells["ID"].Value;
                if (v == DBNull.Value) continue;
                if (Convert.ToInt32(v) == photoId)
                {
                    row.Selected = true;
                    dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }
        #endregion

        #region Сохранение лица в файл (абсолютный путь)
        private void SaveDetectedFaceToDisk(int id, string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);

            string dir = Path.Combine(AppPaths.LogsRoot, DateTime.Now.ToString("yyyy-MM-dd"));
            Directory.CreateDirectory(dir);

            string fn = $"{DateTime.Now:HH-mm-ss}_id{id}.jpg";
            File.WriteAllBytes(Path.Combine(dir, fn), bytes);
        }
        #endregion

        #region Отображение лица
        private void ShowFaceFromBase64(string base64)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                using (var ms = new MemoryStream(bytes))
                {
                    Image img = Image.FromStream(ms);
                    guna2PanelLastCapturedFace.BackgroundImage?.Dispose();
                    guna2PanelLastCapturedFace.BackgroundImage = (Image)img.Clone();
                    guna2PanelLastCapturedFace.BackgroundImageLayout = ImageLayout.Zoom;
                }
            }
            catch { }
        }
        public void ClearSelectedFace()
        {
            guna2PanelSelectedFace.BackgroundImage?.Dispose();
            guna2PanelSelectedFace.BackgroundImage = null;
        }
        #endregion

        #region Выбор строки – показ фото
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var row = dataGridView1.SelectedRows[0];
            object idObj = row.Cells["ID"].Value;
            if (idObj == null || idObj == DBNull.Value) return;
            if (!int.TryParse(idObj.ToString(), out int idFoto)) return;

            int colDate = FindColumnIndex("Дата");
            int colTime = FindColumnIndex("Время");
            if (colDate < 0 || colTime < 0) return;

            DateTime date;
            TimeSpan time;

            object valDate = row.Cells[colDate].Value;
            object valTime = row.Cells[colTime].Value;

            if (valDate is DateTime dt) date = dt.Date;
            else if (!DateTime.TryParse(valDate?.ToString(), out date)) return;

            if (valTime is TimeSpan ts) time = ts;
            else if (!TimeSpan.TryParse(valTime?.ToString(), out time)) return;

            string dir = Path.Combine(AppPaths.LogsRoot, date.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(dir)) return;

            string best = Directory.GetFiles(dir, $"*_id{idFoto}.*")
                .Select(f => new { f, ts = ExtractTimeFromFilename(Path.GetFileName(f)) })
                .Where(x => x.ts.HasValue)
                .OrderBy(x => Math.Abs((x.ts.Value - time).TotalSeconds))
                .FirstOrDefault()?.f;

            if (string.IsNullOrEmpty(best) || !File.Exists(best)) return;

            guna2PanelSelectedFace.BackgroundImage?.Dispose();
            using (var img = Image.FromFile(best))
                guna2PanelSelectedFace.BackgroundImage = (Image)img.Clone();
            guna2PanelSelectedFace.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private int FindColumnIndex(string logicalName)
        {
            var col = dataGridView1.Columns
                       .Cast<DataGridViewColumn>()
                       .FirstOrDefault(c =>
                           string.Equals(c.DataPropertyName, logicalName, StringComparison.OrdinalIgnoreCase) ||
                           string.Equals(c.HeaderText, logicalName, StringComparison.OrdinalIgnoreCase));
            return col == null ? -1 : col.Index;
        }

        private TimeSpan? ExtractTimeFromFilename(string fn)
        {
            var p = fn.Split('_').FirstOrDefault()?.Split('-');
            if (p?.Length == 3 &&
                int.TryParse(p[0], out int h) &&
                int.TryParse(p[1], out int m) &&
                int.TryParse(p[2], out int s))
                return new TimeSpan(h, m, s);
            return null;
        }
        #endregion

        #region Чек-боксы
        private void CheckBoxChanged(object sender, EventArgs e)
        {
            if (sender == guna2CheckBox1 && guna2CheckBox1.Checked)
                guna2CheckBox2.Checked = false;

            if (sender == guna2CheckBox2 && guna2CheckBox2.Checked)
                guna2CheckBox1.Checked = false;

            guna2BtnStartRecognition.Enabled =
                guna2CheckBox1.Checked || guna2CheckBox2.Checked;
        }
        #endregion

        public void DisposeRecognition()
        {
            _recog?.Stop();
            _recog?.Dispose();
            _recog = null;

            _cam.Unsubscribe(OnFrameArrived);
            latestFrame?.Dispose();
            latestFrame = null;

            guna2PictureBoxLiveCamera.Image = null;
            guna2PanelLastCapturedFace.BackgroundImage = null;
            guna2PanelSelectedFace.BackgroundImage = null;

            guna2CheckBox1.Enabled = guna2CheckBox2.Enabled = true;
        }
    }
}
