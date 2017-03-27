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
            //return context.Set<Album>().Select(album => new DalAlbum()
            //{
            //    Id = album.Id,
            //    Name = album.Name,
            //    CreationDate = album.CreationDate,
            //    UserId = album.UserId,
            //    //User = new DalUser()
            //    //{
            //    //    Id = album.User.Id,
            //    //    Email = album.User.Email,
            //    //    Password = album.User.Password,
            //    //    Role = new DalRole()
            //    //    {
            //    //        Id = album.User.Role.Id,
            //    //        Name = album.User.Role.Name
            //    //    }
            //    //},
            //    Images = album.Images.Select(image => new DalImage()
            //    {
            //        Id = image.Id,
            //        Name = image.Name,
            //        Description = image.Description,
            //        Url = image.Url,
            //        Extension = new DalExtension()
            //        {
            //            Id = image.ExtensionId,
            //            Name = image.Extension.Name
            //        }
            //    }).ToList()
            //});
            throw new NotImplementedException();
        }

        public DalAlbum GetById(int key)
        {
            //var album = context.Set<Album>().FirstOrDefault(a => a.Id == key);
            //return new DalAlbum()
            //{
            //    Id = album.Id,
            //    Name = album.Name,
            //    CreationDate = album.CreationDate,
            //    UserId = album.UserId,
            //    //User = new DalUser()
            //    //{
            //    //    Id = album.User.Id,
            //    //    Email = album.User.Email,
            //    //    Password = album.User.Password,
            //    //    Role = new DalRole()
            //    //    {
            //    //        Id = album.User.Role.Id,
            //    //        Name = album.User.Role.Name
            //    //    }
            //    //},
            //    Images = album.Images.Select(image => new DalImage()
            //    {
            //        Id = image.Id,
            //        Name = image.Name,
            //        Description = image.Description,
            //        Url = image.Url,
            //        Extension = new DalExtension()
            //        {
            //            Id = image.ExtensionId,
            //            Name = image.Extension.Name
            //        }
            //    }).ToList()
            //};
            throw new NotImplementedException();
        }

        public IEnumerable<DalAlbum> GetByUserId(int key)
        {
            return context.Set<Album>().Where(a => a.UserId == key).Select(album => new DalAlbum()
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
