using EmployeeManagement.EF.Entity;
using System.Collections.Generic;

namespace EmployeeManagement.EF.Repository.Interface
{
    public interface IProjectRepository
    {
        IEnumerable<ProjectEntity> GetAll();
        ProjectEntity? GetById(int id);
        void Create(ProjectEntity entity);
        void Edit(ProjectEntity entity);
        void Delete(int id);
        void Save();
    }
}
