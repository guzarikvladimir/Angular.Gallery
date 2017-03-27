using System.Collections.Generic;
using DAL.Interfaces.DTO;

namespace DAL.Interfaces.Repository
{
    public interface IImageRepository : IRepository<DalImage>
    {
        IEnumerable<DalImage> GetByAlbumId(int key);
    }
}