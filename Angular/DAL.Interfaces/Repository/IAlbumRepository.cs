using System.Collections.Generic;
using DAL.Interfaces.DTO;

namespace DAL.Interfaces.Repository
{
    public interface IAlbumRepository : IRepository<DalAlbum>
    {
        IEnumerable<DalAlbum> GetByUserId(int key);
    }
}