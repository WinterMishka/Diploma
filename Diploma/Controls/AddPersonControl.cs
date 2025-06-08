#region using
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json;
using Diploma.Data;
using Diploma.Services;
using Diploma.Classes;
#endregion

namespace Diploma
{
    public partial class AddPersonControl : UserControl
    {
        #region Сервисы
        private readonly CameraService _cam = CameraService.Instance;
        private readonly PhotoService _photo = PhotoService.Instance;
        private readonly FaceStorageService _faces = FaceStorageService.Instance;
        private readonly IAppDbService _db;
        #endregion

        #region Поля
        private Bitmap latestFrame;
        #endregion

        #region Конструкторы
        public AddPersonControl()
            : this(new SqlAppDbService(
                   Properties.Settings.Default.EducationAccessSystemConnectionString))
        {
        }

        public AddPersonControl(IAppDbService db)
        {
            InitializeComponent();
            CheckBoxUI.ApplyRecursive(this);
            _db = db;


            LoadReferenceData();
            AttachHandlers();

            flpThumbnails.FlowDirection = FlowDirection.LeftToRight;
            flpThumbnails.WrapContents = true;

            rdoStudent_CheckedChanged(this, EventArgs.Empty);

            this.Load += (s, e) =>
            {
                if (FindForm() is FaceControl face)
                    UiSettingsManager.ApplyTo(face);
            };
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            _cam.Subscribe(OnFrame);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
        }
        #endregion

        #region Загрузка справочников
        private void LoadReferenceData()
        {
            lstCourses.DataSource = _db.GetCourses();
            lstCourses.DisplayMember = "Наименование";
            lstCourses.ValueMember = "id_курса";

            lstGroups.DataSource = _db.GetGroupsFull();
            lstGroups.DisplayMember = "Название_полное";
            lstGroups.ValueMember = "id_группы";

            lstSpecialities.DataSource = _db.GetSpecialities();
            lstSpecialities.DisplayMember = "Название";
            lstSpecialities.ValueMember = "id_специальности";

            lstStatuses.DataSource = _db.GetStatuses();
            lstStatuses.DisplayMember = "Название";
            lstStatuses.ValueMember = "id_статуса";
        }
        #endregion

        #region Подписка
        private void AttachHandlers()
        {
            lstGroups.SelectedIndexChanged += LoadStudentsIfNeeded;
            lstCourses.SelectedIndexChanged += LoadStudentsIfNeeded;
            lstSpecialities.SelectedIndexChanged += LoadStudentsIfNeeded;

            guna2RadioButton1.CheckedChanged += RadioModeChanged;
            guna2RadioButton2.CheckedChanged += RadioModeChanged;

            rdoStudent.CheckedChanged += rdoStudent_CheckedChanged;
            rdoEmployee.CheckedChanged += rdoEmployee_CheckedChanged;
        }
        #endregion

        #region Камера
        private void OnFrame(object s, Bitmap bmp)
        {
            latestFrame?.Dispose();
            latestFrame = (Bitmap)bmp.Clone();

            if (guna2PbLiveCamera.IsHandleCreated &&
                !guna2PbLiveCamera.IsDisposed &&
                !guna2PbLiveCamera.Disposing)
            {
                var disp = (Bitmap)bmp.Clone();
                try
                {
                    if (guna2PbLiveCamera.InvokeRequired)
                    {
                        guna2PbLiveCamera.BeginInvoke((Action)(() =>
                        {
                            if (guna2PbLiveCamera.IsDisposed) { disp.Dispose(); return; }
                            guna2PbLiveCamera.Image?.Dispose();
                            guna2PbLiveCamera.Image = disp;
                        }));
                    }
                    else
                    {
                        guna2PbLiveCamera.Image?.Dispose();
                        guna2PbLiveCamera.Image = disp;
                    }
                }
                catch (InvalidAsynchronousStateException)
                {
                    disp.Dispose();
                }
            }
        }

        public void DisposeCamera()
        {
            _cam.Unsubscribe(OnFrame);
            latestFrame?.Dispose();
            latestFrame = null;
        }

        public void StopCamera() => DisposeCamera();
        #endregion

        #region Снимок
        private void guna2BtnSnap_Click(object sender, EventArgs e)
        {
            if (latestFrame == null) return;

            var shot = (Bitmap)latestFrame.Clone();
            var pb = new PictureBox
            {
                Width = 90,
                Height = 90,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = shot
            };

            flpThumbnails.Controls.Add(pb);
            flpThumbnails.Controls.SetChildIndex(pb, 0);

            if (flpThumbnails.Controls.Count > 9)
            {
                var old = (PictureBox)flpThumbnails.Controls[9];
                flpThumbnails.Controls.RemoveAt(9);
                old?.Image?.Dispose();
                old?.Dispose();
            }
        }
        #endregion

        #region Сохранение
        private async void guna2BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox1.Text) ||
                flpThumbnails.Controls.OfType<PictureBox>().Count(pb => pb.Image != null) < 3)
            {
                MessageBox.Show("Укажите ФИО и сделайте минимум 3 фотографии.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var parts = comboBox1.Text.Trim().Split(' ');
            string fam = parts[0],
                   im = parts.ElementAtOrDefault(1) ?? "",
                   ot = parts.ElementAtOrDefault(2) ?? "";

            var shots = flpThumbnails.Controls
                                     .OfType<PictureBox>()
                                     .Take(3)
                                     .Select(p => p.Image)
                                     .ToArray();

            if (!await ValidatePhotosAsync(shots))
            {
                MessageBox.Show("Фотографии не прошли проверку на сервере.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int photoId = _photo.Save(shots[0]);

            bool isStudent = rdoStudent.Checked;
            int personId;

            if (isStudent)
            {
                if (guna2RadioButton2.Checked && comboBox1.SelectedValue != null)
                {
                    personId = (int)comboBox1.SelectedValue;
                    _photo.AssignToStudent(personId, photoId);
                }
                else
                {
                    int sid = (int)((DataRowView)lstSpecialities.SelectedItem)["id_специальности"];
                    int cid = (int)((DataRowView)lstCourses.SelectedItem)["id_курса"];
                    int gid = (int)((DataRowView)lstGroups.SelectedItem)["id_группы"];

                    personId = _db.AddStudent(fam, im, ot, sid, cid, gid);
                    _photo.AssignToStudent(personId, photoId);
                }
            }
            else
            {
                int statusId = (int)((DataRowView)lstStatuses.SelectedItem)["id_статуса"];
                personId = _db.AddEmployee(fam, im, ot, statusId);
                _photo.AssignToEmployee(personId, photoId);
            }

            bool ok = _faces.SaveSet(photoId, shots,
                                      isStudent ? FaceRole.Student : FaceRole.Staff);

            if (ok)
                await ReloadEncodingsAsync();

            MessageBox.Show(ok ? "Данные успешно сохранены."
                               : "Не удалось сохранить фотографии в Faces.",
                            ok ? "Готово" : "Внимание",
                            MessageBoxButtons.OK,
                            ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }
        #endregion

        #region Очистка
        private void guna2BtnClear_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            comboBox1.DataSource = null;
            comboBox1.Items.Clear();
            comboBox1.Text = "";

            lstGroups.ClearSelected();
            lstCourses.ClearSelected();
            lstSpecialities.ClearSelected();
            lstStatuses.ClearSelected();

            foreach (var pb in flpThumbnails.Controls.OfType<PictureBox>())
            {
                pb.Image?.Dispose();
                pb.Dispose();
            }
            flpThumbnails.Controls.Clear();

            guna2RadioButton1.Checked = true;
            rdoStudent.Checked = true;
            comboBox1.Focus();
        }
        #endregion

        #region Переключатели
        private void RadioModeChanged(object sender, EventArgs e)
        {
            bool fromFile = guna2RadioButton2.Checked;
            comboBox1.DropDownStyle = fromFile ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;

            if (fromFile) LoadStudentsIfNeeded(null, null);
            else
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                comboBox1.Text = "";
            }
        }

        private void rdoStudent_CheckedChanged(object sender, EventArgs e)
        {
            bool s = rdoStudent.Checked;
            lstGroups.Enabled = s;
            lstCourses.Enabled = s;
            lstSpecialities.Enabled = s;
            lstStatuses.Enabled = !s;
        }

        private void rdoEmployee_CheckedChanged(object sender, EventArgs e) =>
            rdoStudent_CheckedChanged(sender, e);
        #endregion

        #region Загрузка студентов
        private void LoadStudentsIfNeeded(object sender, EventArgs e)
        {
            if (!guna2RadioButton2.Checked) return;
            if (lstGroups.SelectedItem == null || lstCourses.SelectedItem == null || lstSpecialities.SelectedItem == null)
                return;

            int gid = (int)((DataRowView)lstGroups.SelectedItem)["id_группы"];
            int cid = (int)((DataRowView)lstCourses.SelectedItem)["id_курса"];
            int sid = (int)((DataRowView)lstSpecialities.SelectedItem)["id_специальности"];

            var tbl = _db.GetStudentsWithoutPhoto(gid, cid, sid);

            comboBox1.DataSource = tbl;
            comboBox1.DisplayMember = "ФИО";
            comboBox1.ValueMember = "id_учащегося";
        }
        #endregion

        #region Валидация фотографий
        private async Task<bool> ValidatePhotosAsync(Image[] shots)
        {
            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
            using (var content = new MultipartFormDataContent())
            {
                for (int i = 0; i < shots.Length; i++)
                {
                    using (var ms = new MemoryStream())
                    {
                        shots[i].Save(ms, ImageFormat.Jpeg);
                        var bytes = ms.ToArray();
                        var part = new ByteArrayContent(bytes);
                        part.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        content.Add(part, "photos", $"shot{i}.jpg");
                    }
                }

                try
                {
                    var resp = await client.PostAsync("http://127.0.0.1:5000/api/validate_photos", content);
                    resp.EnsureSuccessStatusCode();
                    var json = await resp.Content.ReadAsStringAsync();
                    dynamic obj = JsonConvert.DeserializeObject(json);
                    return obj != null && obj.ok == true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static async Task ReloadEncodingsAsync()
        {
            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
            {
                try
                {
                    var resp = await client.PostAsync("http://127.0.0.1:5000/api/reload_encodings", null);
                    resp.EnsureSuccessStatusCode();
                }
                catch { }
            }
        }
        #endregion
    }
}
