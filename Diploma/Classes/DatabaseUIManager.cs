#region using
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Diploma.Data;
#endregion

namespace Diploma.Helpers
{
    public class DatabaseUIManager
    {
        #region Поля
        private readonly IAppDbService _db;
        private readonly FormManager _mgr;
        private readonly CheckedStateManager _state;
        private readonly ComboBox _combo;
        private readonly DataGridView _grid;
        #endregion

        #region Конструктор
        public DatabaseUIManager(IAppDbService db, FormManager mgr, CheckedStateManager state, ComboBox comboBox, DataGridView dataGrid)
        {
            _db = db;
            _mgr = mgr;
            _state = state;
            _combo = comboBox;
            _grid = dataGrid;
        }
        #endregion

        #region Обновление основного DataGridView
        public void UpdateGrid()
        {
            _grid.AutoGenerateColumns = true;

            if (_state.IsGroupMode())
                _grid.DataSource = _db.GetGroupsFull();
            else if (_state.IsCourseMode())
                _grid.DataSource = _db.GetCourses();
            else if (_state.IsSpecialityMode())
                _grid.DataSource = _db.GetSpecialities();
            else if (_state.IsStatusMode())
                _grid.DataSource = _db.GetStatuses();
            else if (_state.IsCuratorMode())
                _grid.DataSource = _db.GetGroupsWithoutCurator();
        }
        #endregion

        #region Настройка ComboBox в зависимости от режима
        public void UpdateComboBoxAfterCheck()
        {
            bool curatorMode = _state.IsCuratorMode();

            if (curatorMode)
            {
                _combo.DataSource = _db.GetCurators();
                _combo.DisplayMember = "ФИО";
                _combo.ValueMember = "id_сотрудника";
                _combo.DropDownStyle = ComboBoxStyle.DropDownList;
                _combo.Visible = true;
            }
            else if (_state.IsGroupMode() || _state.IsCourseMode() || _state.IsSpecialityMode() || _state.IsStatusMode())
            {
                _combo.DataSource = null;
                _combo.Items.Clear();
                _combo.DropDownStyle = ComboBoxStyle.DropDown;
                _combo.Visible = true;
            }
            else
            {
                _combo.Visible = false;
            }

            if (!curatorMode)
                _combo.Text = string.Empty;
        }
        #endregion

        #region Добавление сущностей по режиму
        public void HandleAdd()
        {
            if (_state.IsCuratorMode())
                return;

            if (_state.IsGroupMode())
            {
                string full = _combo.Text.Trim();
                var parts = full.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out int year))
                {
                    int codeId = _mgr.EnsureGroupCode(parts[0].Trim());
                    _mgr.EnsureGroupInstance(codeId, year, null);
                }
            }
            else if (_state.IsCourseMode())
            {
                if (int.TryParse(_combo.Text.Trim(), out int courseNum))
                    _mgr.EnsureCourse(courseNum);
            }
            else if (_state.IsSpecialityMode())
            {
                _mgr.EnsureSpeciality(_combo.Text.Trim());
            }
            else if (_state.IsStatusMode())
            {
                _mgr.EnsureStatus(_combo.Text.Trim());
            }

            _combo.Text = string.Empty;
        }
        #endregion
    }
}
