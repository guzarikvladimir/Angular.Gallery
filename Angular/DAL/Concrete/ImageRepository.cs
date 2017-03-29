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
                Id = image.Id,
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.isTradable.Value,
                CreationDate = image.CreationDate.Value,
                IsBought = image.IsBought,
                UserId = image.UserId,
                Price = image.Price
            });
        }

        public DalImage GetById(int key)
        {
            var image = context.Set<Image>().FirstOrDefault(img => img.Id == key);
            return image == null ? null : new DalImage()
            {
                Id = image.Id,
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.isTradable.Value,
                CreationDate = image.CreationDate.Value,
                IsBought = image.IsBought,
                UserId = image.UserId,
                Price = image.Price
            };
        }

        public IEnumerable<DalImage> GetByUserId(int key)
        {
            return context.Set<Image>().Where(image => image.Album.UserId == key).Select(image => new DalImage()
            {
                Id = image.Id,
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.isTradable.Value,
                CreationDate = image.CreationDate.Value,
                IsBought = image.IsBought,
                UserId = image.UserId,
                Price = image.Price
            });
        }

        public IEnumerable<DalImage> GetByAlbumUserId(int albumId, int userId)
        {
            return context.Set<Image>().Where(image => image.AlbumId == albumId && image.Album.UserId == userId).Select(image => new DalImage()
            {
                Id = image.Id,
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.isTradable.Value,
                CreationDate = image.CreationDate.Value,
                IsBought = image.IsBought,
                UserId = image.UserId,
                Price = image.Price
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
                isTradable = e.IsTradable,
                CreationDate = e.CreationDate,
                IsBought = e.IsBought,
                UserId = e.UserId,
                Price = e.Price
            });
        }

        public void Delete(DalImage e)
        {
            var image = context.Set<Image>().FirstOrDefault(img => img.Id == e.Id);
            if (image != null)
            {
                context.Set<Image>().Remove(image);
            }
        }

        public void Update(DalImage e)
        {
            var image = context.Set<Image>().FirstOrDefault(img => img.Id == e.Id);
            if (image != null)
            {
                image.Name = e.Name;
                image.Description = e.Description;
                image.AlbumId = e.AlbumId;
                image.isTradable = e.IsTradable;
                image.IsBought = e.IsBought;
                image.UserId = e.UserId;
                image.Price = e.Price;
            }
        }
    }
}