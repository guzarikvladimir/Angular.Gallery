using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;

namespace BLL.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository imageRepository;
        private readonly IUnitOfWork uow;

        public ImageService(IImageRepository imageRepository, IUnitOfWork uow)
        {
            this.imageRepository = imageRepository;
            this.uow = uow;
        }

        public List<ImageEntity> GetAll()
        {
            return imageRepository?.GetAll().Select(image => new ImageEntity()
            {
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.IsTradable
            }).ToList();
        }

        public ImageEntity GetById()
        {
            throw new System.NotImplementedException();
        }

        public List<ImageEntity> GetByUserId(int key)
        {
            var images = imageRepository.GetByUserId(key);
            return images?.Select(image => new ImageEntity()
            {
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.IsTradable
            }).ToList();
        }

        public List<ImageEntity> GetByAlbumUserId(int albumId, int userId)
        {
            var images = imageRepository.GetByAlbumUserId(albumId, userId);
            return images?.Select(image => new ImageEntity()
            {
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.IsTradable
            }).ToList();
        }

        public void Create(ImageEntity image)
        {
            imageRepository.Create(new DalImage()
            {
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                IsTradable = image.IsTradable
            });
            uow.Commit();
        }

        public void Delete(ImageEntity image)
        {
            throw new System.NotImplementedException();
        }

        public void Update(ImageEntity image)
        {
            throw new System.NotImplementedException();
        }
    }
}