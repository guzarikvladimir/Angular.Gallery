using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace BLL.Interfaces.Services
{
    public interface IAlbumService
    {
        List<AlbumEntity> GetAll();
        AlbumEntity GetById();
        List<AlbumEntity> GetByUserId(int key);
        void Create(AlbumEntity album);
        void Delete(AlbumEntity album);
        void Update(AlbumEntity album);
    }
}