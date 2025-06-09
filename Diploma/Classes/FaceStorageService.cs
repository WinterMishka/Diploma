using Diploma.Helpers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Diploma.Services
{
    public enum FaceRole { Student, Staff }

    public sealed class FaceStorageService
    {
        #region Синглтон
        private static readonly Lazy<FaceStorageService> _inst =
            new Lazy<FaceStorageService>(() => new FaceStorageService());
        public static FaceStorageService Instance => _inst.Value;
        private FaceStorageService() { }
        #endregion

        #region Путь к датасету
        private static readonly string _root = AppPaths.FacesRoot;
        #endregion

        #region Публичное API
        public bool SaveSet(int personId, Image[] shots, FaceRole role)
        {
            if (shots == null || shots.Length == 0)
                return false;

            string dir = Path.Combine(
                _root,
                role == FaceRole.Student ? "Students" : "Staff",
                personId.ToString());
            Directory.CreateDirectory(dir);

            bool ok = true;
            for (int i = 0; i < shots.Length; i++)
            {
                string fn = Path.Combine(dir, $"{i + 1}.jpg");
                try
                {
                    shots[i].Save(fn, ImageFormat.Jpeg);
                    if (!File.Exists(fn)) ok = false;
                }
                catch
                {
                    ok = false;
                }
            }
            return ok;
        }
        #endregion
    }
}
