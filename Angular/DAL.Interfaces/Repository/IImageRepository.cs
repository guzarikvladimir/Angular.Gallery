using System.Collections.Generic;
using DAL.Interfaces.DTO;

namespace DAL.Interfaces.Repository
{
    public interface IImageRepository : IRepository<DalImage>
    {
        IEnumerable<DalImage> GetByUserId(int key);
        IEnumerable<DalImage> GetByAlbumUserId(int albumId, int userId);
    }
}