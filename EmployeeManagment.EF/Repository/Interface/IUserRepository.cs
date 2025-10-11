using EmployeeManagement.EF.Entity;
using EmployeeManagement.Models;

namespace EmployeeManagement.EF.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<UserEntity> GetAll();
        UserEntity? GetById(int id);
        void Create(UserModel model);
        void Edit(UserModel model);
        void Delete(int id);
        UserEntity? Find(int id);
        UserModel? Detail(int id);
    }
}
