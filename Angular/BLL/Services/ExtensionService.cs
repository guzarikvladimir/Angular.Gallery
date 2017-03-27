using System.Collections.Generic;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;

namespace BLL.Services
{
    public class ExtensionService : IExtensionService
    {
        private readonly IExtensionRepository extensionRepository;
        private readonly IUnitOfWork uow;

        public ExtensionService(IExtensionRepository extensionRepository, IUnitOfWork uow)
        {
            this.extensionRepository = extensionRepository;
            this.uow = uow;
        }

        public List<ExtensionEntity> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public ExtensionEntity GetById()
        {
            throw new System.NotImplementedException();
        }

        public ExtensionEntity GetByName(string key)
        {
            var extension = extensionRepository.GetByName(key);
            return extension == null ? null : new ExtensionEntity()
            {
                Id = extension.Id,
                Name = extension.Name
            };
        }

        public void Create(ExtensionEntity extension)
        {
            extensionRepository.Create(new DalExtension()
            {
                Name = extension.Name
            });
            uow.Commit();
        }

        public void Delete(ExtensionEntity extension)
        {
            throw new System.NotImplementedException();
        }
    }
}