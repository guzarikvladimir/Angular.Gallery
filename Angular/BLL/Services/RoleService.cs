using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.Repository;

namespace BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public List<RoleEntity> GetAll()
        {
            return roleRepository.GetAll().Select(role => new RoleEntity()
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();
        }

        public RoleEntity GetById(int toleId)
        {
            throw new System.NotImplementedException();
        }
    }
}