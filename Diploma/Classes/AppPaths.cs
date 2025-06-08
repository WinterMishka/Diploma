using System;
using System.IO;

namespace Diploma.Services
{
    public static class AppPaths
    {
        #region Корневые директории
        static AppPaths()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dir = Directory.GetParent(baseDir)?.FullName ?? baseDir; // bin
            dir = Directory.GetParent(dir)?.FullName ?? dir;             // project

            ProjectRoot = dir;
            ServerRoot  = Path.Combine(ProjectRoot, "Server");
            FacesRoot   = Path.Combine(ServerRoot, "Faces");
            LogsRoot    = Path.Combine(ServerRoot, "RecognizedLogs");
        }

        public static string ProjectRoot { get; }
        public static string ServerRoot { get; }
        public static string FacesRoot { get; }
        public static string LogsRoot { get; }
        #endregion

        #region Пути к фото
        public static string StudentFaces => Path.Combine(FacesRoot, "Students");
        public static string StaffFaces => Path.Combine(FacesRoot, "Staff");
        #endregion
    }
}
