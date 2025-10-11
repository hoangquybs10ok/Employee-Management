using EmployeeManagement.EF.Entity;
using EmployeeManagement.EF.Repository.Interface;
using EmployeeManagement.EF.TestDb;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.EF.Repository
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
            return _context.Projects.AsNoTracking().ToList();
        }

        public ProjectEntity? GetById(int id)
        {
            return _context.Projects
                .Include(p => p.UserProjects) 
                .FirstOrDefault(p => p.Id == id);
        }

        public void Create(ProjectEntity entity)
        {
            _context.Projects.Add(entity);
        }

        public void Edit(ProjectEntity entity)
        {
            _context.Projects.Update(entity);
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
