#region using
using Diploma.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
#endregion

namespace Diploma.Helpers
{
    /// <summary>
    ///  Логика перезаписи данных в разных таблицах вкладки «Редактировать».
    ///  Поддерживается изменение большинства таблиц, включая группы,
    ///  сотрудников и учащихся.
    /// </summary>
    public class DbUpdateService
    {
        #region Поля
        private readonly FormManager _mgr;
        private readonly DataGridView _grid;
        private readonly Dictionary<string, ComboBox> _boxes;
        private readonly EditCheckedStateManager _editState;
        private readonly Dictionary<int, Action<DataRowView>> _updateActions;
        private readonly DatabaseControl _databaseControl;
        #endregion

        #region Конструктор
        public DbUpdateService(FormManager mgr,
                               DataGridView grid,
                               Dictionary<string, ComboBox> comboBoxes,
                               EditCheckedStateManager editState,
                               DatabaseControl databaseControl)
        {
            _mgr = mgr;
            _grid = grid;
            _boxes = comboBoxes;
            _editState = editState;
            _databaseControl = databaseControl;

            _updateActions = new Dictionary<int, Action<DataRowView>>
            {
                [0] = UpdateGroup,
                [1] = UpdateGroupCode,
                [2] = UpdateCourse,
                [3] = UpdateFace,        // чек-бокс 4 — «Лицо»
                [4] = UpdateEmployee,     // ←-- новый режим
                [5] = UpdateSpeciality, // чекбокс 6 — "Специальность"
                [6] = UpdateStatus,
                [7] = UpdateStudent     // чекбокс 8 — «Учащиеся»
            };
        }
        #endregion
        public void UpdateCheckedIfConfirmed()
        {
            if (_grid.CurrentRow == null || !(_grid.CurrentRow.DataBoundItem is DataRowView row))
            {
                MessageBox.Show("Пожалуйста, выберите строку для изменения.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mode = _editState.GetCheckedIndex();
            if (!_updateActions.TryGetValue(mode, out var act))
            {
                MessageBox.Show("Функция редактирования для этого режима пока не реализована.",
                                "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var changes = new List<string>();

            if (mode == 0)
            {
                AddChange(changes, "Код группы", row["id_код"],
                           GetComboValue("comboBox3", (int)row["id_код"]));
                AddChange(changes, "Год", row["Год"],
                           GetComboValue("comboBox4", (int)row["Год"]));
                AddChange(changes, "Сотрудник", row["id_сотрудника"],
                           GetComboValueNullable("comboBox5", row["id_сотрудника"] as int?));
            }
            else if (mode == 1)
            {
                AddChange(changes, "Код", row["Код"], _boxes["comboBox5"].Text);
            }
            else if (mode == 2)
            {
                AddChange(changes, "Наименование", row["Наименование"], _boxes["comboBox5"].Text);
            }
            else if (mode == 3)
            {
                AddChange(changes, "Путь", row["Путь"], _boxes["comboBox4"].Text);
            }
            else if (mode == 4)
            {
                AddChange(changes, "Фамилия", row["Фамилия"], _boxes["comboBox3"].Text);
                AddChange(changes, "Имя", row["Имя"], _boxes["comboBox4"].Text);
                AddChange(changes, "Отчество", row["Отчество"], _boxes["comboBox5"].Text);
                AddChange(changes, "id_статуса", row["id_статуса"], GetComboValue("comboBox6", Convert.ToInt32(row["id_статуса"])));
                AddChange(changes, "id_фото", row["id_фото"], _boxes["comboBox7"].Text);
            }
            else if (mode == 5)
            {
                AddChange(changes, "Название", row["Название"], _boxes["comboBox5"].Text);
            }
            else if (mode == 6)
            {
                AddChange(changes, "Название", row["Название"], _boxes["comboBox5"].Text);
            }
            else if (mode == 7)
            {
                AddChange(changes, "Фамилия", row["Фамилия"], _boxes["comboBox2"].Text);
                AddChange(changes, "Имя", row["Имя"], _boxes["comboBox3"].Text);
                AddChange(changes, "Отчество", row["Отчество"], _boxes["comboBox4"].Text);
                AddChange(changes, "Фото_сделано", row["Фото_сделано"], _boxes["comboBox5"].Text);
                AddChange(changes, "id_специальности", row["id_специальности"], GetComboValue("comboBox6", Convert.ToInt32(row["id_специальности"])));
                AddChange(changes, "id_курса", row["id_курса"], GetComboValue("comboBox7", Convert.ToInt32(row["id_курса"])));
                AddChange(changes, "id_группы", row["id_группы"], GetComboValue("comboBox8", Convert.ToInt32(row["id_группы"])));
                AddChange(changes, "id_фото", row["id_фото"], _boxes["comboBox9"].Text);
            }

            if (changes.Count == 0)
            {
                MessageBox.Show("Вы не внесли изменений.",
                                "Нет изменений", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string message = "Будут внесены следующие изменения:\n" +
                             string.Join("\n", changes) +
                             "\nВы уверены, что хотите продолжить?";

            if (MessageBox.Show(message, "Подтверждение изменений",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            act(row);

            UpdateSearchGrid();
            _databaseControl.UpdateSearchGrid();

            MessageBox.Show("Изменения успешно применены.",
                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        #region Режим 0 — таблица «Группа»
        private void UpdateGroup(DataRowView row)
        {
            // Логика обновления для таблицы «Группа»
            int oldCode = (int)row["id_код"];
            int oldYear = (int)row["Год"];
            int? oldEmp = row["id_сотрудника"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["id_сотрудника"]);

            // Значения берём из тех же ComboBox, что и на этапе предварительной
            // проверки изменений (AddChange выше)
          int newCode = TryParseInt(_boxes["comboBox3"].Text, oldCode);
          int newYear = TryParseInt(_boxes["comboBox4"].Text, oldYear);
          int? newEmp = string.IsNullOrWhiteSpace(_boxes["comboBox5"].Text)
              ? (int?)null
              : TryParseInt(_boxes["comboBox5"].Text, oldEmp ?? 0);

            // Сохранение изменений в БД
            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE Группа
SET id_код = @code, Год = @year, id_сотрудника = @emp
WHERE id_группы = @id;";
                cmd.Parameters.AddWithValue("@code", newCode);
                cmd.Parameters.AddWithValue("@year", newYear);
                cmd.Parameters.AddWithValue("@emp", newEmp ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id", row["id_группы"]);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Режим 1 — таблица «Группа_код»
        private void UpdateGroupCode(DataRowView row)
        {
            // Логика обновления для таблицы «Группа_код»
            int oldCode = (int)row["id_код"];
            string oldName = row["Код"].ToString();

            string newName = _boxes["comboBox5"].Text;

            // Сохранение изменений в БД
            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE Группа_код
SET Код = @name
WHERE id_код = @oldCode;";
                cmd.Parameters.AddWithValue("@name", newName);
                cmd.Parameters.AddWithValue("@oldCode", oldCode);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Режим 2 — таблица «Курсы»
        private void UpdateCourse(DataRowView row)
        {
            int id = Convert.ToInt32(row["id_курса"]);
            string newName = _boxes["comboBox5"].Text;

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE Курс
SET Наименование = @name
WHERE id_курса = @id;";
                cmd.Parameters.AddWithValue("@name", newName);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Режим 5 — таблица «Специальность»
        private void UpdateSpeciality(DataRowView row)
        {
            int id = Convert.ToInt32(row["id_специальности"]);
            string newName = _boxes["comboBox5"].Text;

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE Специальность
SET Название = @name
WHERE id_специальности = @id;";
                cmd.Parameters.AddWithValue("@name", newName);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region Режим 3 — таблица «Лицо»
        private void UpdateFace(DataRowView row)
        {
            int idFoto = Convert.ToInt32(row["id_фото"]);
            string newPath = _boxes["comboBox4"].Text.Trim();   // Путь берём из comboBox4

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE Лицо
SET Путь_к_фото = @p          -- если колонка называется иначе, поменяйте
WHERE id_фото     = @id;";
                cmd.Parameters.AddWithValue("@p", string.IsNullOrWhiteSpace(newPath)
                                                     ? (object)DBNull.Value
                                                     : newPath);
                cmd.Parameters.AddWithValue("@id", idFoto);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        #region Режим 4 — таблица «Сотрудники»
        private void UpdateEmployee(DataRowView row)
        {
            int idEmp = Convert.ToInt32(row["id_сотрудника"]);
            string last = _boxes["comboBox3"].Text.Trim();
            string first = _boxes["comboBox4"].Text.Trim();
            string mid = _boxes["comboBox5"].Text.Trim();
            int status = GetComboValue("comboBox6", Convert.ToInt32(row["id_статуса"]));
            int fotoId = TryParseInt(_boxes["comboBox7"].Text,
                                       Convert.ToInt32(row["id_фото"]));

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE Сотрудники
SET    Фамилия      = @last,
       Имя          = @first,
       Отчество     = @mid,
       id_статуса   = @st,
       id_фото      = @foto
WHERE  id_сотрудника = @id;";
                cmd.Parameters.AddWithValue("@last", last);
                cmd.Parameters.AddWithValue("@first", first);
                cmd.Parameters.AddWithValue("@mid", string.IsNullOrWhiteSpace(mid)
                                                        ? (object)DBNull.Value : mid);
                cmd.Parameters.AddWithValue("@st", status);
                cmd.Parameters.AddWithValue("@foto", fotoId);
                cmd.Parameters.AddWithValue("@id", idEmp);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Режим 6 — таблица «Статус_должность»
        private void UpdateStatus(DataRowView row)
        {
            int idStatus = Convert.ToInt32(row["id_статуса"]);
            string newName = _boxes["comboBox5"].Text.Trim();

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE Статус_должность
SET    Название = @name
WHERE  id_статуса = @id;";
                cmd.Parameters.AddWithValue("@name", newName);
                cmd.Parameters.AddWithValue("@id", idStatus);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Режим 7 — таблица «Учащиеся»
        private void UpdateStudent(DataRowView row)
        {
            int idStudent = Convert.ToInt32(row["id_учащегося"]);
            string last = _boxes["comboBox2"].Text.Trim();
            string first = _boxes["comboBox3"].Text.Trim();
            string mid = _boxes["comboBox4"].Text.Trim();

            string doneTxt = _boxes["comboBox5"].Text.Trim();
            bool photoDone;
            if (bool.TryParse(doneTxt, out var parsed))
                photoDone = parsed;
            else if (string.Equals(doneTxt, "Да", StringComparison.OrdinalIgnoreCase))
                photoDone = true;
            else if (string.Equals(doneTxt, "Нет", StringComparison.OrdinalIgnoreCase))
                photoDone = false;
            else

            bool photoDone = bool.TryParse(_boxes["comboBox5"].Text, out var done)
                ? done
                : Convert.ToBoolean(row["Фото_сделано"]);



            int speciality = GetComboValue("comboBox6", Convert.ToInt32(row["id_специальности"]));
            int course = GetComboValue("comboBox7", Convert.ToInt32(row["id_курса"]));
            int group = GetComboValue("comboBox8", Convert.ToInt32(row["id_группы"]));

            int? photoId = string.IsNullOrWhiteSpace(_boxes["comboBox9"].Text)
                ? (int?)null
                : TryParseInt(_boxes["comboBox9"].Text, Convert.ToInt32(row["id_фото"]));

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
UPDATE Учащиеся
SET    Фамилия        = @last,
       Имя            = @first,
       Отчество       = @mid,
       Фото_сделано   = @done,
       id_специальности = @spec,
       id_курса         = @course,
       id_группы        = @group,
       id_фото          = @photo
WHERE  id_учащегося    = @id;";

                cmd.Parameters.AddWithValue("@last", last);
                cmd.Parameters.AddWithValue("@first", first);
                cmd.Parameters.AddWithValue("@mid", string.IsNullOrWhiteSpace(mid) ? (object)DBNull.Value : mid);
                cmd.Parameters.AddWithValue("@done", photoDone);
                cmd.Parameters.AddWithValue("@spec", speciality);
                cmd.Parameters.AddWithValue("@course", course);
                cmd.Parameters.AddWithValue("@group", group);
                cmd.Parameters.AddWithValue("@photo", photoId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id", idStudent);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion


        #region Вспомогательные методы
        private static int TryParseInt(string txt, int fallback)
        {
            return int.TryParse(txt, out var v) ? v : fallback;
        }

        private int GetComboValue(string name, int fallback)
        {
            if (!_boxes.TryGetValue(name, out var cb))
                return fallback;

            if (cb.DropDownStyle == ComboBoxStyle.DropDownList &&
                cb.SelectedValue != null)
                return Convert.ToInt32(cb.SelectedValue);

            return TryParseInt(cb.Text, fallback);
        }

        private int? GetComboValueNullable(string name, int? fallback)
        {
            if (!_boxes.TryGetValue(name, out var cb))
                return fallback;

            if (cb.DropDownStyle == ComboBoxStyle.DropDownList)
                return cb.SelectedValue == null
                    ? (int?)null
                    : Convert.ToInt32(cb.SelectedValue);

            string txt = cb.Text;
            if (string.IsNullOrWhiteSpace(txt))
                return null;

            return TryParseInt(txt, fallback ?? 0);
        }

        private static void AddChange(List<string> diff, string cap, object oldV, object newV)
        {
            if (oldV == DBNull.Value) oldV = null;
            if (newV == DBNull.Value) newV = null;

            if (!Equals(oldV, newV))
            {
                string o = oldV == null ? "—" : oldV.ToString();
                string n = newV == null ? "—" : newV.ToString();
                diff.Add($"{cap}: {o} → {n}");
            }
        }

        private void UpdateSearchGrid()
        {
            _grid.DataSource = null;
            _grid.DataSource = _grid.DataSource; // Здесь нужно указать актуальный источник данных
        }
        #endregion
    }
}
