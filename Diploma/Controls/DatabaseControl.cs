#region using
using Diploma.Data;
using Diploma.Helpers;
using Diploma.Classes;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using Diploma.Services;
#endregion

namespace Diploma
{
    public partial class DatabaseControl : UserControl
    {
        #region Сервисы
        private readonly IAppDbService _db;
        private readonly FormManager _mgr;
        private readonly CheckedStateManager _state;
        private readonly DatabaseUIManager _ui;
        private readonly DbEntryService _entry;
        private readonly GridConfigurator _gridCfg;
        private readonly SearchFilterManager _filter;
        #endregion

        #region Поля
        private readonly GridSelectionBinder _binder;
        private readonly EditCheckedStateManager _editState;
        private readonly DbDeleteService _deleter;
        private readonly DbUpdateService _updater;
        private DataTable _currentView;
        #endregion

        #region Конструкторы
        public DatabaseControl()
            : this(new SqlAppDbService(Properties.Settings.Default.EducationAccessSystemConnectionString))
        { }

        public DatabaseControl(IAppDbService db)
        {
            InitializeComponent();
            CheckBoxUI.ApplyRecursive(this);

            dataGridView6.AutoGenerateColumns = false;

            _db = db;
            _mgr = new FormManager(Properties.Settings.Default.EducationAccessSystemConnectionString);

            _state = new CheckedStateManager(new[]
            {
                guna2CheckBox11,
                guna2CheckBox12,
                guna2CheckBox13,
                guna2CheckBox14,
                guna2CheckBox15
            });
            _ui = new DatabaseUIManager(_db, _mgr, _state, comboBox1, dataGridView1);
            _entry = new DbEntryService(_db);
            _deleter = new DbDeleteService(_mgr, dataGridView6);

            _gridCfg = new GridConfigurator(
                dataGridView6,
                new Dictionary<string, ComboBox>
                {
                    ["comboBox2"] = comboBox2,
                    ["comboBox3"] = comboBox3,
                    ["comboBox4"] = comboBox4,
                    ["comboBox5"] = comboBox5,
                    ["comboBox6"] = comboBox6,
                    ["comboBox7"] = comboBox7,
                    ["comboBox8"] = comboBox8,
                    ["comboBox9"] = comboBox9
                },
                new Dictionary<string, Label>
                {
                    ["label2"] = label2,
                    ["label3"] = label3,
                    ["label4"] = label4,
                    ["label5"] = label5,
                    ["label6"] = label6,
                    ["label7"] = label7,
                    ["label8"] = label8,
                    ["label9"] = label9
                }, _db);

            _filter = new SearchFilterManager(new[]
             {
             comboBox2, comboBox3, comboBox4, comboBox5,
             comboBox6, comboBox7, comboBox8, comboBox9
             });


            _binder = new GridSelectionBinder(
                dataGridView6,
                new Dictionary<int, Guna2CheckBox>
                {
                    [1] = guna2CheckBox1,
                    [2] = guna2CheckBox2,
                    [3] = guna2CheckBox3,
                    [4] = guna2CheckBox4,
                    [5] = guna2CheckBox5,
                    [6] = guna2CheckBox6,
                    [7] = guna2CheckBox7,
                    [8] = guna2CheckBox8
                },
                new Dictionary<string, ComboBox>
                {
                    ["comboBox2"] = comboBox2,
                    ["comboBox3"] = comboBox3,
                    ["comboBox4"] = comboBox4,
                    ["comboBox5"] = comboBox5,
                    ["comboBox6"] = comboBox6,
                    ["comboBox7"] = comboBox7,
                    ["comboBox8"] = comboBox8,
                    ["comboBox9"] = comboBox9
                });

            _editState = new EditCheckedStateManager(new Dictionary<int, Guna2CheckBox>
            {
                [1] = guna2CheckBox1,
                [2] = guna2CheckBox2,
                [3] = guna2CheckBox3,
                [4] = guna2CheckBox4,
                [5] = guna2CheckBox5,
                [6] = guna2CheckBox6,
                [7] = guna2CheckBox7,
                [8] = guna2CheckBox8
            });

            var combosForUpdate = new Dictionary<string, ComboBox>
            {
                ["comboBox2"] = comboBox2,
                ["comboBox3"] = comboBox3,
                ["comboBox4"] = comboBox4,
                ["comboBox5"] = comboBox5,
                ["comboBox6"] = comboBox6,
                ["comboBox7"] = comboBox7,
                ["comboBox8"] = comboBox8,
                ["comboBox9"] = comboBox9
            };
            _updater = new DbUpdateService(_mgr, dataGridView6, combosForUpdate, _editState, this);

            _ui.UpdateGrid();

            guna2CheckBox1.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox2.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox3.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox4.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox5.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox6.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox7.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox8.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox11.CheckedChanged += ModeCheckBox_CheckedChanged;
            guna2CheckBox12.CheckedChanged += ModeCheckBox_CheckedChanged;
            guna2CheckBox13.CheckedChanged += ModeCheckBox_CheckedChanged;
            guna2CheckBox14.CheckedChanged += ModeCheckBox_CheckedChanged;
            guna2CheckBox15.CheckedChanged += ModeCheckBox_CheckedChanged;


            this.Load += (s, e) => UpdateSearchGrid();
            dataGridView2.DataSource = _db.GetStudentVisits();
            DataGridViewUI.BeautifyGrid(dataGridView2);

            this.Load += (s, e) =>
            {
                if (FindForm() is FaceControl face)
                    UiSettingsManager.ApplyTo(face);
            };
        }
        #endregion

        #region Обновление таблицы на вкладке «Поиск»
        public void UpdateSearchGrid()
        {
            _binder.Suspend();
            _gridCfg.HideAllFilters();

            if (guna2CheckBox1.Checked)
            {
                _gridCfg.ConfigureGroups(_db.GetGroupsReadable());
                _filter.ApplyDropDownStyles(comboBox3, comboBox5);
            }
            else if (guna2CheckBox2.Checked)
            {
                _gridCfg.ConfigureGroupCodes(_db.GetGroupCodes());
                _filter.ApplyDropDownStyles();
            }
            else if (guna2CheckBox3.Checked)
            {
                _gridCfg.ConfigureCourses(_db.GetCourses());
                _filter.ApplyDropDownStyles();
            }
            else if (guna2CheckBox4.Checked)
            {
                _gridCfg.ConfigureFaces(_db.GetFaces());
                _filter.ApplyDropDownStyles(comboBox3);
            }
            else if (guna2CheckBox5.Checked)
            {
                _gridCfg.ConfigureEmployees(_db.GetEmployeesReadable());
                _filter.ApplyDropDownStyles(comboBox6, comboBox7);
            }
            else if (guna2CheckBox6.Checked)
            {
                _gridCfg.ConfigureSpecialities(_db.GetSpecialities());
                _filter.ApplyDropDownStyles();
            }
            else if (guna2CheckBox7.Checked)
            {
                _gridCfg.ConfigureStatuses(_db.GetStatuses());
                _filter.ApplyDropDownStyles();
            }
            else if (guna2CheckBox8.Checked)
            {
                _gridCfg.ConfigureStudents(_db.GetStudentsReadable());
                _filter.ApplyDropDownStyles(comboBox5, comboBox6, comboBox7, comboBox8, comboBox9);
            }

            _binder.Resume();
        }

        #endregion

        #region Обработка переключения чекбоксов
        private void Guna2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var clicked = sender as Guna2CheckBox;
            var checkBoxes = tableLayoutPanel6.Controls
                .OfType<Guna2CheckBox>()
                .ToList();

            if (!clicked.Checked && checkBoxes.Count(cb => cb.Checked) == 1)
            {
                clicked.CheckedChanged -= Guna2CheckBox_CheckedChanged;
                clicked.Checked = true;
                clicked.CheckedChanged += Guna2CheckBox_CheckedChanged;
                return;
            }

            foreach (var cb in checkBoxes) cb.CheckedChanged -= Guna2CheckBox_CheckedChanged;
            foreach (var cb in checkBoxes) cb.Checked = cb == clicked;
            foreach (var cb in checkBoxes) cb.CheckedChanged += Guna2CheckBox_CheckedChanged;

            UpdateSearchGrid();
        }
        #endregion

        #region Обработчики режимных чекбоксов
        private void ModeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var clicked = sender as Guna2CheckBox;
            var checkBoxes = new List<Guna2CheckBox>
    {
        guna2CheckBox11,
        guna2CheckBox12,
        guna2CheckBox13,
        guna2CheckBox14,
        guna2CheckBox15
    };
            foreach (var cb in checkBoxes)
                cb.CheckedChanged -= ModeCheckBox_CheckedChanged;
            foreach (var cb in checkBoxes)
                cb.Checked = (cb == clicked);
            foreach (var cb in checkBoxes)
                cb.CheckedChanged += ModeCheckBox_CheckedChanged;

            _ui.UpdateGrid();
            _ui.UpdateComboBoxAfterCheck();
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (_state.IsCuratorMode())
            {
                _entry.AssignCuratorIfConfirmed(dataGridView1, comboBox1);
                _ui.UpdateGrid();
                UpdateSearchGrid();
                return;
            }

            _ui.HandleAdd();
            _ui.UpdateGrid();
            UpdateSearchGrid();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            comboBox1.Text = string.Empty;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "Excel (*.xlsx;*.xls)|*.xlsx;*.xls" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;

                var importer = new ExcelImporter(ofd.FileName, _mgr);
                int added = importer.Import();

                MessageBox.Show($"Импорт завершён. Добавлено: {added}",
                                "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ui.UpdateGrid();
                UpdateSearchGrid();
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            _updater.UpdateCheckedIfConfirmed();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            int index = _editState.GetCheckedIndex();
            _deleter.DeleteCheckedIfConfirmed(index);
            UpdateSearchGrid();
        }
        #endregion
        #region Visits viewer (чек-боксы 9,10 + поиск)
        private void InitVisitViewer()
        {
            guna2CheckBox9.CheckedChanged += VisitsCheckBoxChanged;
            guna2CheckBox10.CheckedChanged += VisitsCheckBoxChanged;
            textBox1.TextChanged += SearchBox_TextChanged;

            guna2CheckBox9.Checked = true;
        }

        private void VisitsCheckBoxChanged(object s, EventArgs e)
        {
            guna2CheckBox9.CheckedChanged -= VisitsCheckBoxChanged;
            guna2CheckBox10.CheckedChanged -= VisitsCheckBoxChanged;

            if (s == guna2CheckBox9) guna2CheckBox10.Checked = !guna2CheckBox9.Checked;
            if (s == guna2CheckBox10) guna2CheckBox9.Checked = !guna2CheckBox10.Checked;

            guna2CheckBox9.CheckedChanged += VisitsCheckBoxChanged;
            guna2CheckBox10.CheckedChanged += VisitsCheckBoxChanged;

            LoadVisits();
        }

        private void LoadVisits()
        {
            _currentView = guna2CheckBox9.Checked
                           ? _db.GetStudentVisits()
                           : _db.GetEmployeeVisits();

            dataGridView2.DataSource = _currentView;
            DataGridViewUI.BeautifyGrid(dataGridView2);
            ApplySearchFilter();
        }


        private void SearchBox_TextChanged(object s, EventArgs e) => ApplySearchFilter();

        private void ApplySearchFilter()
        {
            if (_currentView == null) return;

            string raw = textBox1.Text.Trim();
            if (raw.Length == 0)
            {
                dataGridView2.DataSource = _currentView;
                return;
            }
            var words = raw.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(w => w.Replace("'", "''"))
                           .Distinct(StringComparer.OrdinalIgnoreCase)
                           .ToArray();
            var wordFilters = new List<string>();

            foreach (string w in words)
            {
                var perColumn = new List<string>();

                foreach (DataColumn col in _currentView.Columns)
                {
                    string name = $"[{col.ColumnName}]";

                    if (col.DataType == typeof(string))
                        perColumn.Add($"{name} LIKE '%{w}%'");

                    else if (col.DataType == typeof(int) ||
                             col.DataType == typeof(long) ||
                             col.DataType == typeof(short) ||
                             col.DataType == typeof(decimal) ||
                             col.DataType == typeof(double))
                        perColumn.Add($"Convert({name},'System.String') LIKE '%{w}%'");

                    else if (col.DataType == typeof(DateTime))
                        perColumn.Add($"Convert({name},'System.String') LIKE '%{w}%'");
                }

                if (perColumn.Count > 0)
                    wordFilters.Add("(" + string.Join(" OR ", perColumn) + ")");
            }

            string finalFilter = string.Join(" AND ", wordFilters);

            var dv = new DataView(_currentView) { RowFilter = finalFilter };
            dataGridView2.DataSource = dv;
        }

        #endregion

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            const string msg = "Вы уверены, что хотите удалить все данные базы?";
            if (MessageBox.Show(msg, "Подтверждение", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                var tables = new[]
                {
                    "Приход_уход",
                    "Учащиеся",
                    "Сотрудники",
                    "Лицо",
                    "Группа",
                    "Группа_код",
                    "Курс",
                    "Специальность",
                    "Статус_должность",
                    "ПодписчикиТелеграм"
                };

                using (var con = new SqlConnection(Properties.Settings.Default.EducationAccessSystemConnectionString))
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();

                    foreach (var tbl in tables)
                    {
                        cmd.CommandText = $"IF OBJECT_ID(N'[{tbl}]', 'U') IS NOT NULL DELETE FROM [{tbl}];";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = $"IF OBJECT_ID(N'[{tbl}]', 'U') IS NOT NULL DBCC CHECKIDENT(N'[{tbl}]', RESEED, 0);";
                        cmd.ExecuteNonQuery();
                    }
                }

                var encFile = Path.Combine(AppPaths.ServerRoot, "encodings.pkl");
                if (File.Exists(encFile))
                    File.Delete(encFile);

                MessageBox.Show("База данных очищена.", "Готово",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ui.UpdateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении данных: " + ex.Message,
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            const string msg = "Удалить все логи распознавания?";
            if (MessageBox.Show(msg, "Подтверждение", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                if (Directory.Exists(AppPaths.LogsRoot))
                    Directory.Delete(AppPaths.LogsRoot, true);

                MessageBox.Show("Логи удалены.", "Готово",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении логов: " + ex.Message,
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {

        }
    }
}
