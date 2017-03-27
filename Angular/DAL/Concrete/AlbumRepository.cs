using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;
using ORM;

namespace DAL.Concrete
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly DbContext context;

        public AlbumRepository(DbContext context)
        {
            this.context = context;
        }

        public IEnumerable<DalAlbum> GetAll()
        {
            throw new NotImplementedException();
        }

        public DalAlbum GetById(int key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalAlbum> GetByUserId(int key)
        {
            return context.Set<Album>()?.Where(a => a.UserId == key).Select(album => new DalAlbum()
            {
                Id = album.Id,
                Name = album.Name,
                CreationDate = album.CreationDate,
                UserId = album.UserId
            });
        }

        public void Create(DalAlbum e)
        {
            var album = new Album()
            {
                Name = e.Name,
                CreationDate = e.CreationDate,
                UserId = e.UserId
            };
            context.Set<Album>().Add(album);
        }

        public void Delete(DalAlbum e)
        {
            var album = context.Set<Album>().FirstOrDefault(a => a.Id == e.Id);
            context.Set<Album>().Remove(album);
        }

        public void Update(DalAlbum e)
        {
            var album = context.Set<Album>().FirstOrDefault(a => a.Id == e.Id);
            album.Name = e.Name;
        }
    }
}
