using EmployeeManagement.Entity;
using System.Collections.Generic;

namespace EmployeeManagement.Repository
{
    public interface IProjectRepository
    {
        IEnumerable<ProjectEntity> GetAll();
        ProjectEntity? GetById(int id);
        void Create(ProjectEntity project);
        void Edit(ProjectEntity project);
        void Delete(int id);
        void Save();
    }
}
