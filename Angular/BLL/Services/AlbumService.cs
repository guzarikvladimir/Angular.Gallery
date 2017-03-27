using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;

namespace BLL.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository albumRepository;
        private readonly IUnitOfWork uow;

        public AlbumService(IAlbumRepository albumRepository, IUnitOfWork uow)
        {
            this.albumRepository = albumRepository;
            this.uow = uow;
        }

        public List<AlbumEntity> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public AlbumEntity GetById()
        {
            throw new System.NotImplementedException();
        }

        public List<AlbumEntity> GetByUserId(int key)
        {
            return albumRepository.GetByUserId(key).Select(album => new AlbumEntity()
            {
                Id = album.Id,
                Name = album.Name,
                CreationDate = album.CreationDate,
                UserId = album.UserId
            }).ToList();
        }

        public void Create(AlbumEntity album)
        {
            albumRepository.Create(new DalAlbum()
            {
                Name = album.Name,
                UserId = album.UserId,
                CreationDate = album.CreationDate
            });
            uow.Commit();
        }

        public void Delete(AlbumEntity album)
        {
            throw new System.NotImplementedException();
        }

        public void Update(AlbumEntity album)
        {
            throw new System.NotImplementedException();
        }
    }
}