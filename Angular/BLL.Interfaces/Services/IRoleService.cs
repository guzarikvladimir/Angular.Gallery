using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace BLL.Interfaces.Services
{
    public interface IRoleService
    {
        List<RoleEntity> GetAll();
        RoleEntity GetById(int toleId);
    }
}