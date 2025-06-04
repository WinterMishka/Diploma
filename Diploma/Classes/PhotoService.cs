using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Diploma.Services
{
    public sealed class PhotoService
    {
        #region Singleton
        private static readonly Lazy<PhotoService> _inst =
            new Lazy<PhotoService>(() => new PhotoService());
        public static PhotoService Instance => _inst.Value;
        private PhotoService() { }
        #endregion

        #region Путь к папке Faces (абсолютный путь)
        private static readonly string _facesDir = AppPaths.FacesRoot;
        #endregion

        #region API
        public int Save(Image img)
        {
            int id;
            using (var c = new SqlConnection(Properties.Settings.Default.EducationAccessSystemConnectionString))
            {
                c.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO Лицо(Путь_к_фото) OUTPUT INSERTED.id_фото VALUES('')", c))
                {
                    id = (int)cmd.ExecuteScalar();
                }

                string rel = $@"Faces\{id}.jpg";

                using (var upd = new SqlCommand(
                    "UPDATE Лицо SET Путь_к_фото=@p WHERE id_фото=@id", c))
                {
                    upd.Parameters.AddWithValue("@p", rel);
                    upd.Parameters.AddWithValue("@id", id);
                    upd.ExecuteNonQuery();
                }
            }
            return id;
        }

        public void AssignToStudent(int studentId, int photoId)
        {
            using (var c = new SqlConnection(Properties.Settings.Default.EducationAccessSystemConnectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Учащиеся SET id_фото=@p, Фото_сделано=1 WHERE id_учащегося=@s", c))
            {
                cmd.Parameters.AddWithValue("@p", photoId);
                cmd.Parameters.AddWithValue("@s", studentId);
                c.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AssignToEmployee(int employeeId, int photoId)
        {
            using (var c = new SqlConnection(Properties.Settings.Default.EducationAccessSystemConnectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Сотрудники SET id_фото=@p WHERE id_сотрудника=@e", c))
            {
                cmd.Parameters.AddWithValue("@p", photoId);
                cmd.Parameters.AddWithValue("@e", employeeId);
                c.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
    }
}
