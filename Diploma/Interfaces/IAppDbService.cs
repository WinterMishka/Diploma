using System.Data;

namespace Diploma.Data
{
    public interface IAppDbService
    {
        #region Посещения
        void SaveVisit(int photoId, bool isArrival);
        DataTable GetVisitLog();
        (string FullName, string Status) GetPersonInfo(int photoId);
        #endregion

        #region Выборки справочников
        DataTable GetCourses();
        DataTable GetGroups();
        DataTable GetFaces();
        DataTable GetEmployees();
        DataTable GetAllGroupCodes();
        DataTable GetAllYears();
        DataTable GetAllEmployeeIds();
        DataTable GetStudents();
        DataTable GetGroupsFull();
        DataTable GetGroupsWithoutCurator();
        DataTable GetCurators();
        DataTable GetSpecialities();
        DataTable GetStatuses();
        DataTable GetStudentsWithoutPhoto(int groupId, int courseId);
        DataTable GetGroupCodes();
        DataTable GetStudentVisits();
        DataTable GetEmployeeVisits();

        #endregion

        #region Добавление/редактирование лиц
        int AddStudent(string last, string first, string middle,
                       int specialityId, int courseId, int groupId);
        int AddEmployee(string last, string first, string middle, int statusId);
        #endregion

        #region Академический справочник
        int EnsureGroupCode(string code);
        int EnsureGroupInstance(int codeId, int year, int? curatorId = null);
        int EnsureCourse(int num);
        int EnsureSpeciality(string title);
        int EnsureStatus(string title);
        void AssignCurator(int groupId, int curatorId);
        #endregion
    }
}