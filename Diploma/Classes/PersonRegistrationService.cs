using System;
using System.Data;
using System.Drawing;
using Diploma.Data;
using Diploma.Services;

namespace Diploma.Services
{
    public class PersonData
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public Image[] Photos { get; set; }
        public bool IsStudent { get; set; }
        public bool UseExisting { get; set; }
        public int? ExistingPersonId { get; set; }
        public int? SpecialityId { get; set; }
        public int? CourseId { get; set; }
        public int? GroupId { get; set; }
        public int? StatusId { get; set; }
    }

    public class PersonRegistrationService
    {
        private readonly IAppDbService _db;
        private readonly PhotoService _photo;
        private readonly FaceStorageService _faces;

        public PersonRegistrationService(IAppDbService db, PhotoService photo, FaceStorageService faces)
        {
            _db = db;
            _photo = photo;
            _faces = faces;
        }

        public bool Register(PersonData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Photos == null || data.Photos.Length == 0)
                throw new ArgumentException("No photos provided", nameof(data));

            int photoId = _photo.Save(data.Photos[0]);
            int personId;

            if (data.IsStudent)
            {
                if (data.UseExisting && data.ExistingPersonId.HasValue)
                {
                    personId = data.ExistingPersonId.Value;
                    _photo.AssignToStudent(personId, photoId);
                }
                else
                {
                    if (!data.SpecialityId.HasValue || !data.CourseId.HasValue || !data.GroupId.HasValue)
                        throw new ArgumentException("Student data incomplete", nameof(data));

                    personId = _db.AddStudent(data.LastName, data.FirstName, data.MiddleName,
                                              data.SpecialityId.Value, data.CourseId.Value, data.GroupId.Value);
                    _photo.AssignToStudent(personId, photoId);
                }
            }
            else
            {
                if (!data.StatusId.HasValue)
                    throw new ArgumentException("Employee status not specified", nameof(data));

                personId = _db.AddEmployee(data.LastName, data.FirstName, data.MiddleName, data.StatusId.Value);
                _photo.AssignToEmployee(personId, photoId);
            }

            return _faces.SaveSet(photoId, data.Photos,
                                   data.IsStudent ? FaceRole.Student : FaceRole.Staff);
        }
    }
}
