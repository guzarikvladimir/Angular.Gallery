using DAL.Interfaces.DTO;

namespace DAL.Interfaces.Repository
{
    public interface IExtensionRepository : IRepository<DalExtension>
    {
        DalExtension GetByName(string key);
    }
}