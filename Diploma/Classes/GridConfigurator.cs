#region Используемые пространства имён
using Diploma.Data;
using Diploma.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace Diploma.Helpers
{
    public class GridConfigurator
    {
        #region Поля
        private readonly DataGridView _grid;
        private readonly Dictionary<string, ComboBox> _combo;
        private readonly Dictionary<string, Label> _label;
        private readonly IAppDbService _db;
        #endregion

        #region Конструктор
        public GridConfigurator(DataGridView grid,
                                Dictionary<string, ComboBox> comboBoxes,
                                Dictionary<string, Label> labels,
                                IAppDbService db)
        {
            _grid = grid;
            _combo = comboBoxes;
            _label = labels;
            _db = db;
        }
        #endregion

        #region Публичный API
        public void HideAllFilters()
        {
            foreach (var cb in _combo.Values) cb.Visible = false;
            foreach (var lb in _label.Values) lb.Visible = false;
        }

        public void ConfigureGroups(DataTable tbl)
        {
            PrepareGrid(tbl, new[] { "id_группы", "id_код", "Код", "Год", "id_сотрудника", "Куратор" });
            ShowSimpleFilters(new[] { "Код", "Год", "Куратор" }, 3);

            var cbCode = _combo["comboBox3"];
            cbCode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCode.DataSource = _db.GetGroupCodes();
            cbCode.DisplayMember = "Код";
            cbCode.ValueMember = "id_код";
            _combo["comboBox4"].DropDownStyle = ComboBoxStyle.Simple;
            _combo["comboBox4"].DataSource = _db.GetAllYears()
                                   .AsEnumerable()
                                   .Select(r => r.Field<int>("Год"))
                                   .Distinct()
                                   .ToList();
            var cbEmp = _combo["comboBox5"];
            cbEmp.DropDownStyle = ComboBoxStyle.DropDownList;
            var empTbl = _db.GetEmployeesReadable();
            var empList = empTbl.AsEnumerable()
                                .Select(r => new
                                {
                                    Id = r.Field<int>("id_сотрудника"),
                                    Name = string.Join(" ", new[]
                                    {
                                        r.Field<string>("Фамилия"),
                                        r.Field<string>("Имя"),
                                        r.Field<string>("Отчество")
                                    }.Where(s => !string.IsNullOrWhiteSpace(s)))
                                })
                                .ToList();
            cbEmp.DataSource = empList;
            cbEmp.DisplayMember = "Name";
            cbEmp.ValueMember = "Id";
        }

        public void ConfigureGroupCodes(DataTable tbl)
        {
            PrepareGrid(tbl,
                new[] { "id_код", "Код" });

            SetAllComboSimple();
            ShowSingleFilter("label5", "Код:", "comboBox5");
            ClearComboBox("comboBox5");
        }

        public void ConfigureCourses(DataTable tbl)
        {
            PrepareGrid(tbl,
                new[] { "id_курса", "Наименование" });

            SetAllComboSimple();
            ShowSingleFilter("label5", "Наименование:", "comboBox5");
            ClearComboBox("comboBox5");
        }

        public void ConfigureFaces(DataTable tbl)
        {
            PrepareGrid(tbl, new[] { "id_фото", "Путь" });
            ShowSimpleFilters(new[] { "id_фото", "Путь" }, startIndex: 3);
        }

        public void ConfigureEmployees(DataTable tbl)
        {
            PrepareGrid(tbl,
                new[] { "id_сотрудника", "Фамилия", "Имя", "Отчество",
                "id_статуса", "Должность", "id_фото" });

            ShowSimpleFilters(
                new[] { "Фамилия", "Имя", "Отчество", "Должность", "id_фото" },
                startIndex: 3);

            var cbStatus = _combo["comboBox6"];
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            var tblStatus = _db.GetStatuses();
            cbStatus.DataSource = tblStatus;
            cbStatus.DisplayMember = "Название";
            cbStatus.ValueMember = "id_статуса";

        }


        public void ConfigureSpecialities(DataTable tbl)
        {
            PrepareGrid(tbl,
                new[] { "id_специальности", "Название" });

            SetAllComboSimple();
            ShowSingleFilter("label5", "Название:", "comboBox5");
            _combo["comboBox5"].DataSource =
                tbl.AsEnumerable().Select(r => r.Field<string>("Название"))
                                   .Distinct().ToList();
        }

        public void ConfigureStatuses(DataTable tbl)
        {
            PrepareGrid(tbl,
                new[] { "id_статуса", "Название" });

            SetAllComboSimple();
            ShowSingleFilter("label5", "Название:", "comboBox5");
            _combo["comboBox5"].DataSource =
                tbl.AsEnumerable().Select(r => r.Field<string>("Название"))
                                   .Distinct().ToList();
        }

        public void ConfigureStudents(DataTable tbl)
        {
            PrepareGrid(tbl,
                new[]
                {
                    "id_учащегося", "Фамилия", "Имя", "Отчество", "Фото_сделано",
                    "id_специальности", "Специальность", "id_курса", "Курс",
                    "id_группы", "Группа", "id_фото"
                });

            ShowSimpleFilters(new[]
            {
                "Фамилия", "Имя", "Отчество", "Фото_сделано",
                "Специальность", "Курс", "Группа", "id_фото"
            });
            var cbDone = _combo["comboBox5"];
            cbDone.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDone.DataSource = new[]
            {
                new { Text = "Да",  Value = true  },
                new { Text = "Нет", Value = false }
            };
            cbDone.DisplayMember = "Text";
            cbDone.ValueMember = "Value";

            var cbSpec = _combo["comboBox6"];
            cbSpec.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSpec.DataSource = _db.GetSpecialities();
            cbSpec.DisplayMember = "Название";
            cbSpec.ValueMember = "id_специальности";
            var cbCourse = _combo["comboBox7"];
            cbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCourse.DataSource = _db.GetCourses();
            cbCourse.DisplayMember = "Наименование";
            cbCourse.ValueMember = "id_курса";
            var cbGroup = _combo["comboBox8"];
            cbGroup.DropDownStyle = ComboBoxStyle.DropDownList;
            var groupsTbl = _db.GetGroupsReadable();
            var grpList = groupsTbl.AsEnumerable()
                                   .Select(r => new
                                   {
                                       Id = r.Field<int>("id_группы"),
                                       Name = r.Field<string>("Код") + "-" + r.Field<int>("Год")
                                   })
                                   .ToList();
            cbGroup.DataSource = grpList;
            cbGroup.DisplayMember = "Name";
            cbGroup.ValueMember = "Id";
        }
        #endregion

        #region Внутренние вспомогательные методы
        private void PrepareGrid(DataTable tbl, string[] cols)
        {
            _grid.DataSource = null;
            _grid.Columns.Clear();
            _grid.AutoGenerateColumns = false;
            _grid.DataSource = tbl;

            foreach (string col in cols)
            {
                _grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = col,
                    HeaderText = col,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    Visible = !(col.StartsWith("id_") && col != "id_фото")
                });
            }

            DataGridViewUI.BeautifyGrid(_grid);
        }
        private void ShowSimpleFilters(string[] captions, int startIndex = 2)
        {
            var src = _grid.DataSource as DataTable;
            int visual = 0;

            foreach (string col in captions)
            {
                string lbKey = "label" + (startIndex + visual);
                string cbKey = "comboBox" + (startIndex + visual);

                _label[lbKey].Text = col + ":";
                _label[lbKey].Visible = true;

                ComboBox cb = _combo[cbKey];
                cb.Visible = true;
                cb.DropDownStyle = ComboBoxStyle.Simple;

                if (col == "id_фото")
                {
                    cb.DataSource = _db.GetFaces()
                                        .AsEnumerable()
                                        .Select(r => r.Field<int>("id_фото"))
                                        .Distinct()
                                        .ToList();
                }
                else if (src != null && src.Columns.Contains(col))
                {
                    cb.DataSource = src.AsEnumerable()
                                       .Select(r => r[col])
                                       .Distinct()
                                       .ToList();
                }
                else
                {
                    cb.DataSource = null;
                }

                visual++;
            }
        }

        private void ShowSingleFilter(string labelKey, string caption, string comboKey)
        {
            _label[labelKey].Text = caption;
            _label[labelKey].Visible = true;

            _combo[comboKey].Visible = true;
        }

        private void SetAllComboSimple()
        {
            foreach (ComboBox cb in _combo.Values) cb.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void ClearComboBox(string comboKey)
        {
            ComboBox cb = _combo[comboKey];
            cb.DataSource = null;
            cb.Items.Clear();
        }
        #endregion
    }
}
