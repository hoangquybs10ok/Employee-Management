using Microsoft.EntityFrameworkCore;
using EmployeeManagement.EF.Entity;
using EmployeeManagement.Models;
using EmployeeManagement.EF.TestDb;
using EmployeeManagement.EF.Repository.Interface;


namespace EmployeeManagement.EF.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContextTest _context;
        public UserRepository(DbContextTest context)
        {
            _context = context;
        }
        public IEnumerable<UserEntity> GetAll()
        {

            return _context.Users
                .Include(u => u.UserProjects)
                .ThenInclude(up => up.Project)
                .ToList();
        }
        public void Create(UserModel model)
        {

            var user = new UserEntity
            {
                UserName = model.UserName,
                Role = model.Role,
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),

            };
            _context.Users.Add(user);
            _context.SaveChanges();
            if (model.ProjectIds != null && model.ProjectIds.Any())
            {
                foreach (var projectId in model.ProjectIds)
                {
                    _context.UserProjects.Add(new UserProjectEntity()
                    {
                        UserId =user.Id,
                        ProjectId = projectId,
                    });
                }
            }
            _context.SaveChanges();
        }

        public UserEntity? GetById(int id)
        {
            var user = _context.Users
                .Include(u => u.UserProjects)
                .ThenInclude(up => up.Project)
                .FirstOrDefault(u => u.Id == id);

            return user;
        }
        public void Edit(UserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            var user = _context.Users
                .Include(u => u.UserProjects)
                .FirstOrDefault(u => u.Id == model.Id);
            if (user == null)
                throw new KeyNotFoundException($"Không tìm thấy user với Id = {model.Id}");

            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Role = model.Role;
            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }
            if (model.ProjectIds != null)
            {
                user.UserProjects.Clear();
                foreach (var pid in model.ProjectIds)
                {
                    user.UserProjects.Add(new UserProjectEntity { ProjectId = pid });
                }
            }
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public UserEntity? Find(int id)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
        }
        public UserModel? Detail(int id)
        {
            return _context.Users
                .Include(u => u.UserProjects)
                .ThenInclude(up => up.Project)
                .Where(u => u.Id == id)
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    Email = u.Email,
                    ProjectIds = u.UserProjects != null
                        ? u.UserProjects
                            .Where(up => up.ProjectId != 0)
                            .Select(up => up.ProjectId)
                            .ToList()
                        : new List<int>(),
                    Projects = u.UserProjects != null
                        ? u.UserProjects
                            .Where(up => up.Project != null)
                            .Select(up => new ProjectModel
                            {
                                Id = up.Project!.Id,
                                Name = up.Project!.Name
                            }).ToList()
                        : new List<ProjectModel>()
                })
                .AsNoTracking()
                .FirstOrDefault();
        }
        public void Delete(int id)
        {
            var user = _context.Users.Include(u => u.UserProjects).FirstOrDefault(u => u.Id == id);

            if (user == null)
                throw new KeyNotFoundException($"Không tìm thấy user với Id = {id}");
            if (user.UserProjects.Any())
                _context.RemoveRange(user.UserProjects);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
