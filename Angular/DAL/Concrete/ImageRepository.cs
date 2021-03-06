﻿using System;
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
            throw new NotImplementedException();
        }

        public DalImage GetById(int key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalImage> GetByAlbumId(int key)
        {
            return context.Set<Image>().Where(image => image.AlbumId == key).Select(image => new DalImage()
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