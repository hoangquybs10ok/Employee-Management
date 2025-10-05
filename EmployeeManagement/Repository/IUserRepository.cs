using EmployeeManagement.Entity;
using EmployeeManagement.Models;

namespace EmployeeManagement.Repository
{
    public interface IUserRepository
    {
        void Create(UserModel model);
        UserEntity? GetById(int id);
        void Edit(UserModel model);
        UserEntity? Find(int id);
        UserModel? Detail(int id);
        void Delete(int id);
        IEnumerable<UserEntity> GetAll();
    }
}
