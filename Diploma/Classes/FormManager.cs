using System;
using System.Data.SqlClient;

namespace Diploma
{
    public class FormManager
    {
        private readonly string _connStr;

        public FormManager(string connStr)
        {
            _connStr = connStr;
        }

        #region Свойство подключения

        public string ConnStr => _connStr;

        #endregion

        #region Методы обеспечения данных

        public int EnsureGroupCode(string code)
        {
            using (var c = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(
                @"IF EXISTS(SELECT 1 FROM Группа_код WHERE Код=@c)
                      SELECT id_код FROM Группа_код WHERE Код=@c
                  ELSE
                      INSERT INTO Группа_код(Код)
                      OUTPUT INSERTED.id_код
                      VALUES(@c)", c))
            {
                cmd.Parameters.AddWithValue("@c", code);
                c.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int EnsureGroupInstance(int codeId, int year, int? curatorId = null)
        {
            using (var c = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(
                @"IF EXISTS(SELECT 1 FROM Группа WHERE id_код=@code AND Год=@yr)
                      SELECT id_группы FROM Группа WHERE id_код=@code AND Год=@yr
                  ELSE
                      INSERT INTO Группа(id_код,Год,id_сотрудника)
                      OUTPUT INSERTED.id_группы
                      VALUES(@code,@yr,@cur)", c))
            {
                cmd.Parameters.AddWithValue("@code", codeId);
                cmd.Parameters.AddWithValue("@yr", year);
                cmd.Parameters.AddWithValue("@cur", curatorId.HasValue ? (object)curatorId.Value : DBNull.Value);
                c.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int EnsureCourse(int num)
        {
            using (var c = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(
                @"IF EXISTS(SELECT 1 FROM Курс WHERE Наименование=@n)
                      SELECT id_курса FROM Курс WHERE Наименование=@n
                  ELSE
                      INSERT INTO Курс(Наименование)
                      OUTPUT INSERTED.id_курса
                      VALUES(@n)", c))
            {
                cmd.Parameters.AddWithValue("@n", num.ToString());
                c.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int EnsureSpeciality(string title)
        {
            using (var c = new SqlConnection(ConnStr))
            {
                c.Open();
                using (var sel = new SqlCommand(
                    @"SELECT id_специальности FROM Специальность WHERE Название=@n", c))
                {
                    sel.Parameters.AddWithValue("@n", title.Trim());
                    var v = sel.ExecuteScalar();
                    if (v != null) return Convert.ToInt32(v);
                }
                using (var ins = new SqlCommand(
                    @"INSERT INTO Специальность(Название)
                      VALUES(@n);
                      SELECT SCOPE_IDENTITY();", c))
                {
                    ins.Parameters.AddWithValue("@n", title.Trim());
                    return Convert.ToInt32(ins.ExecuteScalar());
                }
            }
        }

        public void EnsureStatus(string title)
        {
            using (var c = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(
                @"IF EXISTS(SELECT 1 FROM Статус_должность WHERE Название=@n)
                      SELECT id_статуса FROM Статус_должность WHERE Название=@n
                  ELSE
                      INSERT INTO Статус_должность(Название)
                      VALUES(@n)", c))
            {
                cmd.Parameters.AddWithValue("@n", title.Trim());
                c.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Методы управления куратором

        public void AssignCurator(int groupId, int curatorId)
        {
            using (var c = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(
                "UPDATE dbo.Группа SET id_сотрудника = @cur WHERE id_группы = @gid", c))
            {
                cmd.Parameters.AddWithValue("@cur", curatorId);
                cmd.Parameters.AddWithValue("@gid", groupId);
                c.Open();
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Проверки

        public bool StudentExists(int gid, string surname, string name, string patronymic)
        {
            using (var c = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand(
                @"SELECT COUNT(*) FROM Учащиеся
                  WHERE id_группы=@g
                    AND Фамилия=@s
                    AND Имя=@n
                    AND (Отчество=@p OR (Отчество IS NULL AND @p IS NULL))
                    AND Фото_сделано=0", c))
            {
                cmd.Parameters.AddWithValue("@g", gid);
                cmd.Parameters.AddWithValue("@s", surname);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@p", (object)patronymic ?? DBNull.Value);
                c.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        #endregion

        #region Методы выборки фото
        public int? GetPhotoIdForStudent(int studentId)
        {
            using (var c = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand("SELECT id_фото FROM Учащиеся WHERE id_учащегося = @id", c))
            {
                cmd.Parameters.AddWithValue("@id", studentId);
                c.Open();
                var val = cmd.ExecuteScalar();
                return val == DBNull.Value ? null : (int?)val;
            }
        }

        public int? GetPhotoIdForEmployee(int employeeId)
        {
            using (var c = new SqlConnection(ConnStr))
            using (var cmd = new SqlCommand("SELECT id_фото FROM Сотрудники WHERE id_сотрудника = @id", c))
            {
                cmd.Parameters.AddWithValue("@id", employeeId);
                c.Open();
                var val = cmd.ExecuteScalar();
                return val == DBNull.Value ? null : (int?)val;
            }
        }
        #endregion
    }
}
