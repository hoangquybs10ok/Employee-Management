using EmployeeManagement.Entity;
using EmployeeManagement.Models;

namespace EmployeeManagement.Repository
{
    public interface ITimeLogRepository
    {
        void Create(TimeLogModel model);
        TimeLogEntity? GetById(int id);
        void Edit(TimeLogModel model);
        TimeLogEntity? Find(int id);
        TimeLogModel? Detail(int id);
        void Delete(int id);
        IEnumerable<TimeLogEntity> GetAll();
    }
}
