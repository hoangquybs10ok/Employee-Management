using Microsoft.EntityFrameworkCore;
using EmployeeManagement.EF.Entity;
using EmployeeManagement.Models;
using EmployeeManagement.EF.TestDb;
using EmployeeManagement.EF.Repository.Interface;

namespace EmployeeManagement.EF.Repository
{
    public class TimeLogRepository : ITimeLogRepository
    {
        private readonly DbContextTest _context;

        public TimeLogRepository(DbContextTest context)
        {
            _context = context;
        }

        public void Create(TimeLogModel model)
        {
            var entity = new TimeLogEntity
            {
                UserId = model.UserId,
                CheckIn = model.CheckIn,
                CheckOut = model.CheckOut,
                Description = model.Description,
                Task = model.Task,
            };
            _context.TimeLogs.Add(entity);
            _context.SaveChanges();
        }

        public TimeLogEntity? GetById(int id)
        {
            return _context.TimeLogs.FirstOrDefault(x => x.Id == id);
        }

        public void Edit(TimeLogModel model)
        {
            var entity = _context.TimeLogs.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null) return;

            entity.UserId = model.UserId;
            entity.CheckIn = model.CheckIn;
            entity.CheckOut = model.CheckOut;
            entity.Description = model.Description;
            entity.Task = model.Task;

            _context.SaveChanges();
        }

        public TimeLogEntity? Find(int id)
        {
            return _context.TimeLogs.Find(id);
        }

        public TimeLogModel? Detail(int id)
        {
            return _context.TimeLogs
                .Include(x => x.User)
                .Where(x => x.Id == id)
                .Select(x => new TimeLogModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    CheckIn = x.CheckIn,
                    CheckOut = x.CheckOut,
                    Description = x.Description,
                    Task = x.Task,
                    UserName = x.User!.FullName,
                }).FirstOrDefault();
        }
        public void Delete(int id)
        {
            var entity = _context.TimeLogs.FirstOrDefault(x => x.Id == id);
            if (entity == null) return;

            _context.TimeLogs.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<TimeLogEntity> GetAll()
        {
            return _context.TimeLogs.AsNoTracking().ToList();
        }
    }
}
