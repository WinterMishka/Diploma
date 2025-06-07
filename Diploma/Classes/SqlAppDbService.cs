using System;
using System.Data;
using System.Data.SqlClient;

namespace Diploma.Data
{
    public sealed class SqlAppDbService : IAppDbService
    {
        #region Поля
        private readonly string _conn;
        #endregion

        #region Конструктор
        public SqlAppDbService(string connectionString)
        {
            _conn = connectionString;
        }
        #endregion

        #region Вспомогательные методы
        private DataTable LoadTable(string sql)
        {
            var tbl = new DataTable();
            using (var ad = new SqlDataAdapter(sql, _conn))
            {
                ad.Fill(tbl);
            }
            return tbl;
        }
        #endregion

        #region Посещения
        public void SaveVisit(int photoId, bool isArrival)
        {
            var d = DateTime.Now.Date;
            var t = DateTime.Now.TimeOfDay;

            const string pre = @"
DECLARE @stud INT = (SELECT id_учащегося  FROM Учащиеся   WHERE id_фото=@idFoto);
DECLARE @empl INT = (SELECT id_сотрудника FROM Сотрудники WHERE id_фото=@idFoto);";

            var sql = isArrival
                ? pre + @"
INSERT INTO Приход_уход(дата_прихода, время_прихода, id_сотрудника, id_учащегося)
VALUES(@d,@t,@empl,@stud);"
                : pre + @"
;WITH cte AS (
    SELECT TOP 1 * FROM Приход_уход
    WHERE дата_ухода IS NULL
      AND ((@stud IS NOT NULL AND id_учащегося=@stud)
           OR (@empl IS NOT NULL AND id_сотрудника=@empl))
    ORDER BY id_прихода_ухода DESC)
UPDATE cte SET дата_ухода=@d, время_ухода=@t;

IF @@ROWCOUNT = 0
    INSERT INTO Приход_уход(дата_ухода, время_ухода, id_сотрудника, id_учащегося)
    VALUES(@d,@t,@empl,@stud);";

            using (var c = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@idFoto", photoId);
                cmd.Parameters.AddWithValue("@d", d);
                cmd.Parameters.AddWithValue("@t", t);
                c.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetVisitLog()
        {
            return LoadTable("EXEC ПолучитьПосещения");
        }

        public (string FullName, string Status) GetPersonInfo(int photoId)
        {
            const string sql = @"
SELECT TOP 1
       LTRIM(RTRIM(
           COALESCE(s.Фамилия ,u.Фамилия ) + ' ' +
           COALESCE(s.Имя     ,u.Имя     ) + ' ' +
           COALESCE(NULLIF(s.Отчество,''), NULLIF(u.Отчество,''), '')
       ))                                       AS ФИО,
       ISNULL(st.Название, N'Студент')          AS Статус
FROM   Лицо l
LEFT JOIN Сотрудники      s  ON s.id_фото = l.id_фото
LEFT JOIN Статус_должность st ON st.id_статуса = s.id_статуса
LEFT JOIN Учащиеся        u  ON u.id_фото = l.id_фото
WHERE  l.id_фото = @id;";

            using (var c = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@id", photoId);
                c.Open();
                using (var r = cmd.ExecuteReader())
                {
                    return r.Read()
                        ? (r.GetString(0), r.GetString(1))
                        : ("-", "Студент");
                }
            }
        }
        #endregion

        #region Выборки справочников
        public DataTable GetCourses() => LoadTable("SELECT id_курса, Наименование FROM Курс ORDER BY Наименование");
        public DataTable GetGroups() => LoadTable("SELECT id_группы, id_код, Год, id_сотрудника FROM Группа ORDER BY id_группы");
        public DataTable GetGroupsFull() => LoadTable("SELECT * FROM vw_ГруппыПолные ORDER BY Название_полное");
        public DataTable GetGroupsWithoutCurator() => LoadTable("SELECT * FROM vw_ГруппыБезКуратора ORDER BY Название_полное");
        public DataTable GetCurators() => LoadTable("SELECT * FROM vw_Кураторы ORDER BY ФИО");
        public DataTable GetSpecialities() => LoadTable("SELECT id_специальности, Название FROM Специальность ORDER BY Название");
        public DataTable GetStatuses() => LoadTable("SELECT id_статуса, Название FROM Статус_должность ORDER BY Название");

        public DataTable GetStudentsWithoutPhoto(int groupId, int courseId)
        {
            const string sql = @"
SELECT id_учащегося,
       Фамилия + ' ' + Имя + ' ' + ISNULL(Отчество,'') AS ФИО
FROM   Учащиеся
WHERE  id_группы=@g AND id_курса=@c AND Фото_сделано=0
ORDER  BY Фамилия, Имя;";

            var tbl = new DataTable();
            using (var ad = new SqlDataAdapter(sql, _conn))
            {
                ad.SelectCommand.Parameters.AddWithValue("@g", groupId);
                ad.SelectCommand.Parameters.AddWithValue("@c", courseId);
                ad.Fill(tbl);
            }
            return tbl;
        }
        #endregion

        #region Добавление/редактирование лиц
        public int AddStudent(string last, string first, string middle,
                              int specialityId, int courseId, int groupId)
        {
            const string sql = @"
INSERT INTO Учащиеся(Фамилия,Имя,Отчество,
                     id_специальности,id_курса,id_группы,Фото_сделано)
OUTPUT INSERTED.id_учащегося
VALUES(@l,@f,@m,@sid,@cid,@gid,0);";

            using (var c = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@l", last);
                cmd.Parameters.AddWithValue("@f", first);
                cmd.Parameters.AddWithValue("@m", (object)middle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sid", specialityId);
                cmd.Parameters.AddWithValue("@cid", courseId);
                cmd.Parameters.AddWithValue("@gid", groupId);
                c.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int AddEmployee(string last, string first, string middle, int statusId)
        {
            const string sql = @"
INSERT INTO Сотрудники(Фамилия,Имя,Отчество,id_статуса)
OUTPUT INSERTED.id_сотрудника
VALUES(@l,@f,@m,@sid);";

            using (var c = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@l", last);
                cmd.Parameters.AddWithValue("@f", first);
                cmd.Parameters.AddWithValue("@m", (object)middle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sid", statusId);
                c.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        #endregion

        #region Академический справочник
        public int EnsureGroupCode(string code)
        {
            const string sql = @"
IF EXISTS(SELECT 1 FROM Группа_код WHERE Код=@c)
    SELECT id_код FROM Группа_код WHERE Код=@c
ELSE
    INSERT INTO Группа_код(Код)
    OUTPUT INSERTED.id_код
    VALUES(@c);";

            using (var c = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@c", code);
                c.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int EnsureGroupInstance(int codeId, int year, int? curatorId = null)
        {
            const string sql = @"
IF EXISTS(SELECT 1 FROM Группа WHERE id_код=@code AND Год=@yr)
    SELECT id_группы FROM Группа WHERE id_код=@code AND Год=@yr
ELSE
    INSERT INTO Группа(id_код,Год,id_сотрудника)
    OUTPUT INSERTED.id_группы
    VALUES(@code,@yr,@cur);";

            using (var c = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(sql, c))
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
            const string sql = @"
IF EXISTS(SELECT 1 FROM Курс WHERE Наименование=@n)
    SELECT id_курса FROM Курс WHERE Наименование=@n
ELSE
    INSERT INTO Курс(Наименование)
    OUTPUT INSERTED.id_курса
    VALUES(@n);";

            using (var c = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@n", num.ToString());
                c.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int EnsureSpeciality(string title)
        {
            using (var c = new SqlConnection(_conn))
            {
                c.Open();

                using (var sel = new SqlCommand(
                    "SELECT id_специальности FROM Специальность WHERE Название=@n", c))
                {
                    sel.Parameters.AddWithValue("@n", title.Trim());
                    var v = sel.ExecuteScalar();
                    if (v != null) return Convert.ToInt32(v);
                }

                using (var ins = new SqlCommand(@"
INSERT INTO Специальность(Название)
VALUES(@n);
SELECT SCOPE_IDENTITY();", c))
                {
                    ins.Parameters.AddWithValue("@n", title.Trim());
                    return Convert.ToInt32(ins.ExecuteScalar());
                }
            }
        }

        public int EnsureStatus(string title)
        {
            using (var c = new SqlConnection(_conn))
            {
                c.Open();

                using (var sel = new SqlCommand(
                    "SELECT id_статуса FROM Статус_должность WHERE Название=@n", c))
                {
                    sel.Parameters.AddWithValue("@n", title.Trim());
                    var v = sel.ExecuteScalar();
                    if (v != null) return Convert.ToInt32(v);
                }

                using (var ins = new SqlCommand(@"
INSERT INTO Статус_должность(Название)
VALUES(@n);
SELECT SCOPE_IDENTITY();", c))
                {
                    ins.Parameters.AddWithValue("@n", title.Trim());
                    return Convert.ToInt32(ins.ExecuteScalar());
                }
            }
        }

        public void AssignCurator(int groupId, int curatorId)
        {
            const string sql = @"
UPDATE Группа
SET    id_сотрудника=@cur
WHERE  id_группы=@g;";

            using (var c = new SqlConnection(_conn))
            using (var cmd = new SqlCommand(sql, c))
            {
                cmd.Parameters.AddWithValue("@cur", curatorId);
                cmd.Parameters.AddWithValue("@g", groupId);
                c.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public DataTable GetGroupCodes()
        {
            using (var conn = new SqlConnection(_conn))
            using (var adapter = new SqlDataAdapter("SELECT id_код, Код FROM Группа_код", conn))
            {
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
        public DataTable GetFaces()
        {
            const string sql = @"
        SELECT  f.id_фото,
                CASE
                    WHEN st.id_фото IS NOT NULL THEN
                         CONCAT('Faces\\Students\\', f.id_фото, '.jpg')
                    WHEN sf.id_фото IS NOT NULL THEN
                         CONCAT('Faces\\Staff\\',    f.id_фото, '.jpg')
                    ELSE
                         CONCAT('Faces\\',          f.id_фото, '.jpg')
                END AS Путь
        FROM    Лицо           AS f
        LEFT JOIN Учащиеся     AS st ON st.id_фото = f.id_фото
        LEFT JOIN Сотрудники    AS sf ON sf.id_фото = f.id_фото
        ORDER BY f.id_фото";

            return LoadTable(sql);
        }

        public DataTable GetEmployees()
        {
            return LoadTable(@"SELECT id_сотрудника, Фамилия, Имя, Отчество, id_статуса, id_фото FROM Сотрудники ORDER BY Фамилия, Имя");
        }
        public DataTable GetStudents()
        {
            return LoadTable("SELECT * FROM Учащиеся");
        }
        public DataTable GetAllYears()
        {
            return LoadTable("SELECT DISTINCT Год FROM Группа ORDER BY Год");
        }
        public DataTable GetStudentVisits()
        {
            return LoadTable("SELECT * FROM vw_ПосещенияСтуденты ORDER BY Дата DESC, Время DESC");
        }

        public DataTable GetEmployeeVisits()
        {
            return LoadTable("SELECT * FROM vw_ПосещенияСотрудники ORDER BY Дата DESC, Время DESC");
        }

        public DataTable GetGroupsReadable()
        {
            const string sql = @"
SELECT g.id_группы,
       g.id_код,
       c.Код,
       g.Год,
       g.id_сотрудника,
       CONCAT(emp.Фамилия, ' ', emp.Имя, ' ', ISNULL(emp.Отчество,'')) AS Куратор
FROM   Группа AS g
       INNER JOIN Группа_код AS c ON c.id_код = g.id_код
       LEFT JOIN Сотрудники AS emp ON emp.id_сотрудника = g.id_сотрудника
ORDER BY c.Код, g.Год;";

            return LoadTable(sql);
        }

        public DataTable GetEmployeesReadable()
        {
            const string sql = @"
SELECT e.id_сотрудника,
       e.Фамилия,
       e.Имя,
       e.Отчество,
       e.id_статуса,
       s.Название AS Должность,
       e.id_фото
FROM   Сотрудники AS e
       LEFT JOIN Статус_должность AS s ON s.id_статуса = e.id_статуса
ORDER BY e.Фамилия, e.Имя;";

            return LoadTable(sql);
        }

        public DataTable GetStudentsReadable()
        {
            const string sql = @"
SELECT st.id_учащегося,
       st.Фамилия,
       st.Имя,
       st.Отчество,
       CASE WHEN st.Фото_сделано=1 THEN N'Да' ELSE N'Нет' END AS Фото_сделано,
       st.id_специальности,
       sp.Название         AS Специальность,
       st.id_курса,
       cr.Наименование     AS Курс,
       st.id_группы,
       gc.Код + '-' + CAST(gr.Год AS NVARCHAR(4)) AS Группа,
       st.id_фото
FROM   Учащиеся AS st
       LEFT JOIN Специальность  AS sp ON sp.id_специальности = st.id_специальности
       LEFT JOIN Курс           AS cr ON cr.id_курса = st.id_курса
       LEFT JOIN Группа         AS gr ON gr.id_группы = st.id_группы
       LEFT JOIN Группа_код     AS gc ON gc.id_код = gr.id_код
ORDER BY st.Фамилия, st.Имя;";

            return LoadTable(sql);
        }
        #endregion
    }
}
