// ExcelImporter.cs
using ClosedXML.Excel;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace Diploma
{
    public class ExcelImporter
    {
        private readonly string _filePath;
        private readonly FormManager _mgr;

        private static readonly Regex rxSpec = new Regex("«([^»]+)»", RegexOptions.Compiled);
        private static readonly Regex rxNum = new Regex(@"(\d+)\s*курс", RegexOptions.IgnoreCase);

        public ExcelImporter(string filePath, FormManager manager)
        {
            _filePath = filePath;
            _mgr = manager;
        }

        #region Импорт из Excel

        public int Import()
        {
            var added = 0;
            using (var wb = new XLWorkbook(_filePath))
            {
                foreach (var ws in wb.Worksheets)
                {
                    var sidL = ParseSpeciality(ws, 1, 1, 3);
                    var sidR = ParseSpeciality(ws, 1, 4, 7);

                    ParseCourse(ws.Cell(2, 2).GetString(), out var curL);
                    ParseCourse(ws.Cell(2, 5).GetString(), out var curR);

                    var np = ws.Name
                               .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => x.Trim())
                               .ToArray();

                    var partL = np.Length >= 2 ? np[0] : np[0];
                    var partR = np.Length >= 2 ? np[1] : np[0];

                    if (np.Length < 2)
                    {
                        var m = Regex.Matches(ws.Cell(2, 2).GetString(),
                                                 @"[A-Za-zА-Яа-яЁё]+\s*-\s*\d+(?:-\s*\d+)?");
                        var defs = m.Cast<Match>().Select(x => x.Value.Trim()).ToList();
                        if (defs.Count == 1) defs.Add(defs[0]);
                        partL = defs[0];
                        partR = defs[1];
                    }

                    ParseGroup(partL, out var codeL, out var yearL);
                    ParseGroup(partR, out var codeR, out var yearR);

                    var codeIdL = _mgr.EnsureGroupCode(codeL);
                    var grpIdL = _mgr.EnsureGroupInstance(codeIdL, yearL, null);
                    var codeIdR = _mgr.EnsureGroupCode(codeR);
                    var grpIdR = _mgr.EnsureGroupInstance(codeIdR, yearR, null);

                    var cidL = _mgr.EnsureCourse(curL);
                    var cidR = _mgr.EnsureCourse(curR);

                    for (var row = 5; ; row++)
                    {
                        var l = ImportCell(ws.Cell(row, 2), grpIdL, cidL, sidL, ref added);
                        var r = ImportCell(ws.Cell(row, 5), grpIdR, cidR, sidR, ref added);
                        if (!l && !r) break;
                    }
                }
            }
            return added;
        }

        #endregion

        #region Вспомогательные методы парсинга

        private int ParseSpeciality(IXLWorksheet ws, int r, int from, int to)
        {
            for (var c = from; c <= to; c++)
            {
                var m = rxSpec.Match(ws.Cell(r, c).GetString());
                if (m.Success) return _mgr.EnsureSpeciality(m.Groups[1].Value);
            }
            return _mgr.EnsureSpeciality("Не указано");
        }

        private void ParseCourse(string line, out int course)
        {
            course = 1;
            var m = rxNum.Match(line);
            if (m.Success) course = int.Parse(m.Groups[1].Value);
        }

        private void ParseGroup(string part, out string code, out int year)
        {
            var p = part.Split('-');
            code = p[0].Trim();
            year = int.Parse(p[1].Trim());
        }

        private bool ImportCell(IXLCell cell, int gid, int cid, int sid, ref int added)
        {
            var raw = Regex.Replace(cell.GetString(), @"\s+", " ").Trim();
            if (string.IsNullOrEmpty(raw) ||
                raw.IndexOf("академ", StringComparison.OrdinalIgnoreCase) >= 0)
                return raw != "";

            var w = raw.Split(' ');
            if (w.Length < 2) return true;

            var patr = w.Length >= 3 ? w.Last() : null;
            var name = w.Length >= 3 ? w[w.Length - 2] : w.Last();
            var sur = w.Length >= 3
                       ? string.Join(" ", w.Take(w.Length - 2))
                       : w[0];

            if (_mgr.StudentExists(gid, sur, name, patr))
                return true;

            using (var c = new SqlConnection(_mgr.ConnStr))
            using (var cmd = new SqlCommand(
                @"INSERT INTO Учащиеся
                   (Фамилия,Имя,Отчество,id_специальности,id_курса,id_группы,Фото_сделано)
                  VALUES(@s,@n,@p,@sid,@cid,@gid,0)", c))
            {
                cmd.Parameters.AddWithValue("@s", sur);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@p", (object)patr ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sid", sid);
                cmd.Parameters.AddWithValue("@cid", cid);
                cmd.Parameters.AddWithValue("@gid", gid);
                c.Open();
                cmd.ExecuteNonQuery();
            }

            added++;
            return true;
        }

        #endregion
    }
}
