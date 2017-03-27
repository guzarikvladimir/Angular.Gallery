using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace BLL.Interfaces.Services
{
    public interface IExtensionService
    {
        List<ExtensionEntity> GetAll();
        ExtensionEntity GetById();
        ExtensionEntity GetByName(string key);
        void Create(ExtensionEntity extension);
        void Delete(ExtensionEntity extension);
    }
}