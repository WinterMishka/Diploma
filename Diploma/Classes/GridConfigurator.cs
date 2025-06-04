#region using
using Diploma.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace Diploma.Helpers
{
    /// <summary>Отвечает за конфигурацию dataGridView6 и видимость фильтров.</summary>
    public class GridConfigurator
    {
        #region Поля
        private readonly DataGridView _grid;
        private readonly Dictionary<string, ComboBox> _combo;
        private readonly Dictionary<string, Label> _label;
        private readonly IAppDbService _db;
        #endregion

        #region .ctor
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

            // фильтры начинаются с label3/comboBox3
            ShowSimpleFilters(new[] { "Код", "Год", "Куратор" }, 3);

            /* ---------- заполняем из таблиц БД ---------- */

            // id_код  → comboBox3 (DropDownList)
            var cbCode = _combo["comboBox3"];
            cbCode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCode.DataSource = _db.GetAllGroupCodes()
                                   .AsEnumerable()
                                   .Select(r => r.Field<int>("id_код"))
                                   .Distinct()
                                   .ToList();

            // Год  → comboBox4  (оставляем текстовое поле - Simple)
            _combo["comboBox4"].DropDownStyle = ComboBoxStyle.Simple;
            _combo["comboBox4"].DataSource = _db.GetAllYears()
                                   .AsEnumerable()
                                   .Select(r => r.Field<int>("Год"))
                                   .Distinct()
                                   .ToList();

            // id_сотрудника → comboBox5 (DropDownList)
            var cbEmp = _combo["comboBox5"];
            cbEmp.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEmp.DataSource = _db.GetAllEmployeeIds()
                                  .AsEnumerable()
                                  .Select(r => r.Field<int>("id_сотрудника"))
                                  .Distinct()
                                  .ToList();
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
            // колонки грида
            PrepareGrid(tbl, new[] { "id_фото", "Путь" });

            // фильтры с label3 / comboBox3
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

            /* ----- id_статуса → comboBox6 из таблицы Статус_должность ----- */
            var cbStatus = _combo["comboBox6"];
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            var tblStatus = _db.GetStatuses();           // SELECT id_статуса, Название …
            cbStatus.DataSource = tblStatus;
            cbStatus.DisplayMember = "Название";
            cbStatus.ValueMember = "id_статуса";

            /* id_фото можно сделать аналогично, если понадобится */
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
                    Visible = !col.StartsWith("id_")
                });
            }
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

                if (src != null && src.Columns.Contains(col))
                    cb.DataSource = src.AsEnumerable()
                                       .Select(r => r[col])
                                       .Distinct()
                                       .ToList();
                else
                    cb.DataSource = null;

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
