#region using
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Diploma.Helpers;
using Guna.UI2.WinForms;
#endregion

namespace Diploma
{
    public class GridSelectionBinder
    {
        #region Поля
        private readonly DataGridView _grid;
        private readonly Dictionary<int, Guna2CheckBox> _checkBoxes;
        private readonly Dictionary<string, ComboBox> _comboBoxes;
        private readonly EditCheckedStateManager _editState;
        private readonly Dictionary<int, Action<DataRowView>> _bindingActions;
        #endregion

        #region Конструктор
        public GridSelectionBinder(
            DataGridView grid,
            Dictionary<int, Guna2CheckBox> checkBoxes,
            Dictionary<string, ComboBox> comboBoxes)
        {
            _grid = grid;
            _checkBoxes = checkBoxes;
            _comboBoxes = comboBoxes;
            _editState = new EditCheckedStateManager(checkBoxes);

            _bindingActions = new Dictionary<int, Action<DataRowView>>
            {
                [0] = BindGroup,
                [1] = BindGroupCode,
                [2] = BindCourseOrSpecialty,
                [3] = BindFace,
                [4] = BindEmployee,
                [5] = BindSpecialty,
                [6] = BindStatus,
                [7] = BindStudent
            };

            _grid.SelectionChanged += OnGridSelectionChanged;
        }
        #endregion

        #region Управление подпиской
        public void Suspend()
        {
            _grid.SelectionChanged -= OnGridSelectionChanged;
        }

        public void Resume()
        {
            _grid.SelectionChanged += OnGridSelectionChanged;
        }
        #endregion

        #region Привязка по выбору строки
        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            if (_grid.CurrentRow == null) return;
            if (!(_grid.CurrentRow.DataBoundItem is DataRowView row)) return;

            int index = _editState.GetCheckedIndex();
            if (_bindingActions.TryGetValue(index, out var action))
                action.Invoke(row);
        }
        #endregion

        #region Методы привязки
        private void BindGroup(DataRowView row)
        {
            SetCombo("comboBox3", row, "id_код");
            SetCombo("comboBox4", row, "Год");
            SetCombo("comboBox5", row, "id_сотрудника");
        }


        private void BindGroupCode(DataRowView row)
        {
            SetCombo("comboBox5", row, "Код");
        }

        private void BindCourseOrSpecialty(DataRowView row)
        {
            SetCombo("comboBox5", row, "Наименование");
        }

        private void BindFace(DataRowView row)
        {
            SetCombo("comboBox3", row, "id_фото");

            SetCombo("comboBox4", row, "Путь");
        }

        private void BindEmployee(DataRowView row)
        {
            SetCombo("comboBox3", row, "Фамилия");
            SetCombo("comboBox4", row, "Имя");
            SetCombo("comboBox5", row, "Отчество");
            SetCombo("comboBox6", row, "id_статуса");
            SetCombo("comboBox7", row, "id_фото");
        }

        private void BindSpecialty(DataRowView row)
        {
            SetCombo("comboBox5", row, "Название");
        }

        private void BindStatus(DataRowView row)
        {
            SetCombo("comboBox5", row, "Название");
        }

        private void BindStudent(DataRowView row)
        {
            SetCombo("comboBox2", row, "Фамилия");
            SetCombo("comboBox3", row, "Имя");
            SetCombo("comboBox4", row, "Отчество");
            SetCombo("comboBox5", row, "Фото_сделано");
            SetCombo("comboBox6", row, "id_специальности");
            SetCombo("comboBox7", row, "id_курса");
            SetCombo("comboBox8", row, "id_группы");
            SetCombo("comboBox9", row, "id_фото");
        }
        #endregion

        #region Вспомогательный метод
        private void SetCombo(string comboName, DataRowView row, string columnName)
        {
            if (!_comboBoxes.TryGetValue(comboName, out var box)) return;
            if (!row.DataView.Table.Columns.Contains(columnName)) return;

            object value = row[columnName];


            if (value == DBNull.Value ||
        (value is string s && string.IsNullOrWhiteSpace(s)) ||
        (value is int i && i == 0))
            {
                box.SelectedIndex = -1;
                box.Text = string.Empty;
            }
            else
            {

                if (box.DropDownStyle == ComboBoxStyle.DropDownList &&
                    box.DataSource != null && !string.IsNullOrEmpty(box.ValueMember))
                {
                    try
                    {
                        box.SelectedValue = value;
                    }
                    catch (FormatException)
                    {
                        box.SelectedIndex = -1;
                        box.Text = value.ToString();
                    }
                }
                else if (box.Items.Contains(value))
                {
                    box.SelectedItem = value;
                }
                else
                {
                    box.Text = value.ToString();
                }
            }
        }

        #endregion
    }
}
