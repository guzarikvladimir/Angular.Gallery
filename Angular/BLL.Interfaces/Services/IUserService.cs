using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace BLL.Interfaces.Services
{
    public interface IUserService
    {
        List<UserEntity> GetAll();
        UserEntity GetById();
        UserEntity GetByEmail(string email);
        void Create(UserEntity user);
        void Delete(UserEntity user);
        void Update(UserEntity user);
    }
}