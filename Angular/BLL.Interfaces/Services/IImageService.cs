using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace BLL.Interfaces.Services
{
    public interface IImageService
    {
        List<ImageEntity> GetAll();
        ImageEntity GetById();
        List<ImageEntity> GetByAlbumId(int key);
        void Create(ImageEntity image);
        void Delete(ImageEntity image);
        void Update(ImageEntity image);
    }
}