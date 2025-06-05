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
using System.Windows.Forms;
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

            // запретим автоматическое создание колонок, чтобы
            // вручную контролировать их видимость
            dataGridView6.AutoGenerateColumns = false;

            _db = db;
            _mgr = new FormManager(Properties.Settings.Default.EducationAccessSystemConnectionString);

            _state = new CheckedStateManager(checkedListBox1);
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

            checkedListBox1.CheckOnClick = true;
            _ui.UpdateGrid();

            guna2CheckBox1.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox2.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox3.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox4.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox5.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox6.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox7.CheckedChanged += (s, e) => UpdateSearchGrid();
            guna2CheckBox8.CheckedChanged += (s, e) => UpdateSearchGrid();

            this.Load += (s, e) => UpdateSearchGrid();
            dataGridView2.DataSource = _db.GetStudentVisits();
        }
        #endregion

        #region Обновление таблицы на вкладке «Поиск»
        public void UpdateSearchGrid()
        {
            _binder.Suspend();
            _gridCfg.HideAllFilters();

            if (guna2CheckBox1.Checked)                 // Группа
            {
                _gridCfg.ConfigureGroups(_db.GetGroupsReadable());
                _filter.ApplyDropDownStyles(comboBox3, comboBox5);        // id_код, id_сотрудника
            }
            else if (guna2CheckBox2.Checked)            // Код
            {
                _gridCfg.ConfigureGroupCodes(_db.GetGroupCodes());
                _filter.ApplyDropDownStyles();                                   // все текстовые
            }
            else if (guna2CheckBox3.Checked)            // Курс
            {
                _gridCfg.ConfigureCourses(_db.GetCourses());
                _filter.ApplyDropDownStyles();                                   // все текстовые
            }
            else if (guna2CheckBox4.Checked)          // Лицо
            {
                _gridCfg.ConfigureFaces(_db.GetFaces());
                _filter.ApplyDropDownStyles(comboBox3);
            }
            else if (guna2CheckBox5.Checked)            // Сотрудники
            {
                _gridCfg.ConfigureEmployees(_db.GetEmployeesReadable());
                _filter.ApplyDropDownStyles(comboBox6, comboBox7);
            }
            else if (guna2CheckBox6.Checked)            // Специальность
            {
                _gridCfg.ConfigureSpecialities(_db.GetSpecialities());
                _filter.ApplyDropDownStyles();                                   // все текстовые
            }
            else if (guna2CheckBox7.Checked)            // Статус
            {
                _gridCfg.ConfigureStatuses(_db.GetStatuses());
                _filter.ApplyDropDownStyles();                                   // все текстовые
            }
            else if (guna2CheckBox8.Checked)            // Учащиеся
            {
                _gridCfg.ConfigureStudents(_db.GetStudentsReadable());
                _filter.ApplyDropDownStyles(comboBox5, comboBox6, comboBox7, comboBox8, comboBox9); // Фото_сделано, id_специальности, id_курса, id_группы, id_фото
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

        #region Обработчики кнопок
        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            _state.EnforceSingleCheck(e);
            BeginInvoke(new MethodInvoker(() =>
            {
                _ui.UpdateGrid();
                _ui.UpdateComboBoxAfterCheck();
            }));
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (_state.IsCuratorMode())
            {
                _entry.AssignCuratorIfConfirmed(dataGridView1, comboBox1);
                _ui.UpdateGrid();
                return;
            }

            _ui.HandleAdd();
            _ui.UpdateGrid();
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

            guna2CheckBox9.Checked = true;   // стартуем со студентов
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
                dataGridView2.DataSource = _currentView;              // сбросить фильтр
                return;
            }

            // 1. Разбили строку на слова, убрали дубли.
            var words = raw.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(w => w.Replace("'", "''"))
                           .Distinct(StringComparer.OrdinalIgnoreCase)
                           .ToArray();

            // 2. Для каждого слова собираем (col LIKE '%word%' OR …) по ВСЕМ колонкам
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

    }
}
