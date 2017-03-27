using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;
using ORM;

namespace DAL.Concrete
{
    public class ImageRepository : IImageRepository
    {
        private readonly DbContext context;

        public ImageRepository(DbContext context)
        {
            this.context = context;
        }

        public IEnumerable<DalImage> GetAll()
        {
            return context.Set<Image>().Select(image => new DalImage()
            {
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.isTradable.Value
            });
        }

        public DalImage GetById(int key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalImage> GetByUserId(int key)
        {
            return context.Set<Image>().Where(image => image.Album.UserId == key).Select(image => new DalImage()
            {
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.isTradable.Value
            });
        }

        public IEnumerable<DalImage> GetByAlbumUserId(int albumId, int userId)
        {
            return context.Set<Image>().Where(image => image.AlbumId == albumId && image.Album.UserId == userId).Select(image => new DalImage()
            {
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.isTradable.Value
            });
        }

        public void Create(DalImage e)
        {
            context.Set<Image>().Add(new Image()
            {
                Name = e.Name,
                Description = e.Description,
                AlbumId = e.AlbumId,
                ExtensionId = e.ExtensionId,
                isTradable = e.IsTradable
            });
        }

        public void Delete(DalImage e)
        {
            throw new NotImplementedException();
        }

        public void Update(DalImage e)
        {
            throw new NotImplementedException();
        }
    }
}