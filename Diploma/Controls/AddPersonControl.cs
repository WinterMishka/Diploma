#region using
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Diploma.Data;
using Diploma.Services;
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
        private readonly PersonRegistrationService _personSvc;
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
            _db = db;
            _personSvc = new PersonRegistrationService(_db, _photo, _faces);

            Disposed += (s, e) => DisposeCamera();

            LoadReferenceData();
            AttachHandlers();

            flpThumbnails.FlowDirection = FlowDirection.LeftToRight;
            flpThumbnails.WrapContents = true;

            _cam.Subscribe(OnFrame);
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

            if (guna2PbLiveCamera.IsHandleCreated)
            {
                var disp = (Bitmap)bmp.Clone();
                guna2PbLiveCamera.Invoke((Action)(() =>
                {
                    guna2PbLiveCamera.Image?.Dispose();
                    guna2PbLiveCamera.Image = disp;
                }));
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
        private void guna2BtnSave_Click(object sender, EventArgs e)
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
                   im = parts.ElementAtOrDefault(1) ?? string.Empty,
                   ot = parts.ElementAtOrDefault(2) ?? string.Empty;

            var shots = flpThumbnails.Controls
                                     .OfType<PictureBox>()
                                     .Take(3)
                                     .Select(p => p.Image)
                                     .ToArray();

            var data = new PersonData
            {
                LastName = fam,
                FirstName = im,
                MiddleName = ot,
                Photos = shots,
                IsStudent = rdoStudent.Checked,
                UseExisting = guna2RadioButton2.Checked,
                ExistingPersonId = (guna2RadioButton2.Checked && comboBox1.SelectedValue != null)
                                   ? (int?)comboBox1.SelectedValue
                                   : null,
                SpecialityId = lstSpecialities.SelectedItem == null
                               ? (int?)null
                               : (int)((DataRowView)lstSpecialities.SelectedItem)["id_специальности"],
                CourseId = lstCourses.SelectedItem == null
                           ? (int?)null
                           : (int)((DataRowView)lstCourses.SelectedItem)["id_курса"],
                GroupId = lstGroups.SelectedItem == null
                          ? (int?)null
                          : (int)((DataRowView)lstGroups.SelectedItem)["id_группы"],
                StatusId = lstStatuses.SelectedItem == null
                          ? (int?)null
                          : (int)((DataRowView)lstStatuses.SelectedItem)["id_статуса"]
            };

            bool ok = _personSvc.Register(data);

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
            if (lstGroups.SelectedItem == null || lstCourses.SelectedItem == null) return;

            int gid = (int)((DataRowView)lstGroups.SelectedItem)["id_группы"];
            int cid = (int)((DataRowView)lstCourses.SelectedItem)["id_курса"];

            var tbl = _db.GetStudentsWithoutPhoto(gid, cid);

            comboBox1.DataSource = tbl;
            comboBox1.DisplayMember = "ФИО";
            comboBox1.ValueMember = "id_учащегося";
        }
        #endregion
    }
}
