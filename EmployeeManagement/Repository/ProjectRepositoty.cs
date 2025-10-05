using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using EmployeeManagement.Entity;
using EmployeeManagement.TestDb;

namespace EmployeeManagement.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DbContextTest _context;

        public ProjectRepository(DbContextTest context)
        {
            _context = context;
        }

        public IEnumerable<ProjectEntity> GetAll()
        {
            return _context.Projects
                           .Include(p => p.UserProjects) 
                           .ThenInclude(up => up.User)   
                           .ToList();
        }
        public ProjectEntity? GetById(int id)
        {
            return _context.Projects
                           .Include(p => p.UserProjects)
                           .ThenInclude(up => up.User)
                           .FirstOrDefault(p => p.Id == id);
        }
        public void Create(ProjectEntity project)
        {
            _context.Projects.Add(project);
        }
        public void Edit(ProjectEntity project)
        {
            _context.Projects.Update(project);
        }
        public void Delete(int id)
        {
            var project = _context.Projects.Find(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
