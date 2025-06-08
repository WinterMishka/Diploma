using System;
using System.IO;

namespace Diploma.Services
{
    public static class AppPaths
    {
        #region Корневые директории
        // Вычисляем корень проекта относительно директории запуска приложения
        public static string ProjectRoot =>
            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                          "..", ".."));
        public static string ServerRoot => Path.Combine(ProjectRoot, "Server");
        public static string FacesRoot => Path.Combine(ServerRoot, "Faces");
        public static string LogsRoot => Path.Combine(ServerRoot, "RecognizedLogs");
        #endregion

        #region Пути к фото
        public static string StudentFaces => Path.Combine(FacesRoot, "Students");
        public static string StaffFaces => Path.Combine(FacesRoot, "Staff");
        #endregion
    }
}
