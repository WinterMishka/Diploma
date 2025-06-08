#region using
using Diploma.Data;
using Diploma.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace Diploma.Helpers
{
    public class DbDeleteService
    {
        #region Поля
        private readonly FormManager _mgr;
        private readonly DataGridView _grid;
        private readonly Dictionary<int, Action<DataRowView>> _deleteActions;
        private readonly string _studentPath = AppPaths.StudentFaces;
        private readonly string _staffPath = AppPaths.StaffFaces;
        #endregion

        #region Конструктор
        public DbDeleteService(FormManager mgr, DataGridView grid)
        {
            _mgr = mgr;
            _grid = grid;

            _deleteActions = new Dictionary<int, Action<DataRowView>>
            {
                [0] = row => DeleteWithCascade(row, "Группа", "id_группы", "..."),
                [1] = row => DeleteWithCascade(row, "Группа_код", "id_код", "..."),
                [2] = row => DeleteWithCascade(row, "Курс", "id_курса", "..."),
                [3] = row => DeleteWithCascade(row, "Лицо", "id_фото", "..."),
                [4] = DeleteStaffWithPhoto,
                [5] = row => DeleteWithCascade(row, "Специальность", "id_специальности", "..."),
                [6] = row => DeleteWithCascade(row, "Статус_должность", "id_статуса", "..."),
                [7] = DeleteStudentWithPhoto
            };

        }
        #endregion

        #region Методы удаления

        private void DeleteStudentWithPhoto(DataRowView row)
        {
            string fio = row["Фамилия"] + " " + row["Имя"] + " " + row["Отчество"];
            var confirm = MessageBox.Show("Вы уверены, что хотите удалить учащегося:\n\n" + fio + "\n\nДействие необратимо.",
                                          "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            int id = (int)row["id_учащегося"];
            DeleteStudentById(id);

            MessageBox.Show("Учащийся успешно удалён.",
                            "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteStudentById(int studentId)
        {
            int? photoId = _mgr.GetPhotoIdForStudent(studentId);

            if (photoId.HasValue)
            {
                string jpgPath = Path.Combine(_studentPath, photoId + ".jpg");
                string folderPath = Path.Combine(_studentPath, photoId.ToString());

                DeleteVisits("id_учащегося", studentId);
                DeleteFaceRecord(photoId.Value);
                DeleteFileIfExists(jpgPath);
                DeleteFolderIfExists(folderPath);
            }

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("DELETE FROM Учащиеся WHERE id_учащегося = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", studentId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteStaffWithPhoto(DataRowView row)
        {
            int id = (int)row["id_сотрудника"];
            int? photoId = _mgr.GetPhotoIdForEmployee(id);

            var warn = MessageBox.Show(
                "Все группы, учащиеся и записи о приходах, связанные с сотрудником, будут удалены. Продолжить?",
                "Удаление данных", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (warn != DialogResult.Yes) return;

            DeleteGroupsForCurator(id);
            ClearGroupCurators(id);

            DeleteVisits("id_сотрудника", id);

            if (photoId.HasValue)
            {
                string jpgPath = Path.Combine(_staffPath, photoId + ".jpg");
                string folderPath = Path.Combine(_staffPath, photoId.ToString());

                DeleteFaceRecord(photoId.Value);
                DeleteFileIfExists(jpgPath);
                DeleteFolderIfExists(folderPath);
            }

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("DELETE FROM Сотрудники WHERE id_сотрудника = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void ClearGroupCurators(int employeeId)
        {
            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("UPDATE Группа SET id_сотрудника = NULL WHERE id_сотрудника = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", employeeId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteVisits(string column, int id)
        {
            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("DELETE FROM Приход_уход WHERE " + column + " = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteFaceRecord(int photoId)
        {
            using (var con = new SqlConnection(_mgr.ConnStr))
            {
                con.Open();

                using (var cmd1 = new SqlCommand("UPDATE Учащиеся SET id_фото = NULL WHERE id_фото = @id", con))
                {
                    cmd1.Parameters.AddWithValue("@id", photoId);
                    cmd1.ExecuteNonQuery();
                }

                using (var cmd2 = new SqlCommand("UPDATE Сотрудники SET id_фото = NULL WHERE id_фото = @id", con))
                {
                    cmd2.Parameters.AddWithValue("@id", photoId);
                    cmd2.ExecuteNonQuery();
                }

                using (var cmd3 = new SqlCommand("DELETE FROM Лицо WHERE id_фото = @id", con))
                {
                    cmd3.Parameters.AddWithValue("@id", photoId);
                    cmd3.ExecuteNonQuery();
                }
            }
        }

        private void DeleteWithCascade(DataRowView row, string table, string idColumn, string cascadeWarning)
        {
            if (!row.Row.Table.Columns.Contains(idColumn))
            {
                string cols = string.Join(", ", row.Row.Table.Columns
                                                            .Cast<DataColumn>()
                                                            .Select(c => c.ColumnName));
                MessageBox.Show($"Колонка '{idColumn}' отсутствует в таблице.\n\nДоступные колонки: {cols}",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = (int)row[idColumn];

            if (table == "Курс")
            {
                var confirm = MessageBox.Show("Все учащиеся с этим курсом будут удалены. Продолжить?",
                                              "Удаление курса", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

                DeleteStudentsWhere("id_курса", id);
            }
            else if (table == "Специальность")
            {
                var confirm = MessageBox.Show("Все учащиеся с этой специальностью будут удалены. Продолжить?",
                                              "Удаление специальности", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

                DeleteStudentsWhere("id_специальности", id);
            }
            else if (table == "Группа")
            {
                var confirm = MessageBox.Show("Все учащиеся в этой группе будут удалены. Продолжить?",
                                              "Удаление группы", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

                DeleteStudentsWhere("id_группы", id);
            }
            else if (table == "Группа_код")
            {
                var confirm = MessageBox.Show("Все группы и учащиеся с этим кодом будут удалены. Продолжить?",
                                              "Удаление кода группы", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

                DeleteGroupsByCode(id);
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить запись из " + table + "?\nID: " + id + "\n\n" + cascadeWarning,
                                         "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return;

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("DELETE FROM " + table + " WHERE " + idColumn + " = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Запись успешно удалена.",
                            "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteStudentsWhere(string column, int value)
        {
            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("SELECT id_учащегося FROM Учащиеся WHERE " + column + " = @val", con))
            {
                cmd.Parameters.AddWithValue("@val", value);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    List<int> ids = new List<int>();
                    while (reader.Read())
                        ids.Add(reader.GetInt32(0));

                    foreach (int id in ids)
                        DeleteStudentById(id);
                }
            }
        }

        private void DeleteGroupsByCode(int codeId)
        {
            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("SELECT id_группы FROM Группа WHERE id_код = @code", con))
            {
                cmd.Parameters.AddWithValue("@code", codeId);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    List<int> groupIds = new List<int>();
                    while (reader.Read())
                        groupIds.Add(reader.GetInt32(0));

                    foreach (int groupId in groupIds)
                        DeleteStudentsWhere("id_группы", groupId);
                }
            }

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("DELETE FROM Группа WHERE id_код = @code", con))
            {
                cmd.Parameters.AddWithValue("@code", codeId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteGroupsForCurator(int curatorId)
        {
            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("SELECT id_группы FROM Группа WHERE id_сотрудника = @cur", con))
            {
                cmd.Parameters.AddWithValue("@cur", curatorId);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var ids = new List<int>();
                    while (reader.Read())
                        ids.Add(reader.GetInt32(0));

                    foreach (int gid in ids)
                        DeleteStudentsWhere("id_группы", gid);
                }
            }

            using (var con = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand("DELETE FROM Группа WHERE id_сотрудника = @cur", con))
            {
                cmd.Parameters.AddWithValue("@cur", curatorId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCheckedIfConfirmed(int modeIndex)
        {
            if (_grid.CurrentRow == null || !(_grid.CurrentRow.DataBoundItem is DataRowView))
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = (DataRowView)_grid.CurrentRow.DataBoundItem;

            if (_deleteActions.ContainsKey(modeIndex))
                _deleteActions[modeIndex](row);
            else
                MessageBox.Show("Неизвестный режим удаления.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DeleteFileIfExists(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления файла: " + ex.Message,
                                "Файл не удалён", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteFolderIfExists(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления папки: " + ex.Message,
                                "Папка не удалена", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}
