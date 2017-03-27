using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserRepository userRepository;

        public UserService(IUnitOfWork uow, IUserRepository userRepository)
        {
            this.uow = uow;
            this.userRepository = userRepository;
        }

        public List<UserEntity> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public UserEntity GetById()
        {
            throw new System.NotImplementedException();
        }

        public UserEntity GetByEmail(string email)
        {
            var user = userRepository.GetAll().FirstOrDefault(u => u.Email == email);
            return user == null ? null : new UserEntity()
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId,
                Role = new RoleEntity()
                {
                    Id = user.RoleId,
                    Name = user.Role.Name
                }
            };
        }

        public void Create(UserEntity user)
        {
            userRepository.Create(new DalUser()
            {
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId
            });
            uow.Commit();
        }

        public void Delete(UserEntity user)
        {
            throw new System.NotImplementedException();
        }

        public void Update(UserEntity user)
        {
            throw new System.NotImplementedException();
        }
    }
}