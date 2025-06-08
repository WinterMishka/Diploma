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

            // Move up from .../bin/Debug/netX/ to the project directory
            var dir = Directory.GetParent(baseDir)?.FullName ?? baseDir; // net
            dir = Directory.GetParent(dir)?.FullName ?? dir;             // Debug
            dir = Directory.GetParent(dir)?.FullName ?? dir;             // bin

            ProjectRoot = dir;
            ServerRoot  = Path.Combine(ProjectRoot, "Server");
            FacesRoot   = Path.Combine(ServerRoot, "Faces");
            LogsRoot    = Path.Combine(ServerRoot, "RecognizedLogs");

            // Ensure all required directories exist
            Directory.CreateDirectory(Path.Combine(FacesRoot, "Students"));
            Directory.CreateDirectory(Path.Combine(FacesRoot, "Staff"));
            Directory.CreateDirectory(LogsRoot);
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
