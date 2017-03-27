using Angular.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Angular.Infrastructure;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;

namespace Angular.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageService imageService;
        private readonly IExtensionService extensionService;

        public ImageController(IImageService imageService, IExtensionService extensionService)
        {
            this.imageService = imageService;
            this.extensionService = extensionService;
        }

        public JsonResult GetImages()
        {
            var images = imageService.GetAll();
            return Json(images.Select(image => new Image()
            {
                Id = image.Id,
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                Url = Path.Combine("\\Content", "img", image.Name)
            }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFileAjax(string url)
        {
            var path = Server.MapPath(url);
            System.IO.File.Delete(path);
            return Json(true);
        }

        public JsonResult GetImagesForAlbum(int albumId, int userId)
        {
            var images = albumId == int.MaxValue ? 
                imageService.GetByUserId(userId) : 
                imageService.GetByAlbumUserId(albumId, userId);
            return Json(images.Select(image => new Image()
            {
                Id = image.Id,
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                Url = Path.Combine("\\Content", "img", image.Name)
            }));
        }

        public JsonResult AddImageAjax(string fileName, string data, string description, int albumId, bool isTradable)
        {
            var dataIndex = data.IndexOf("base64", StringComparison.Ordinal) + 7;
            var cleareData = data.Substring(dataIndex);
            var fileData = Convert.FromBase64String(cleareData);
            var bytes = fileData.ToArray();

            var path = GetPathToImg(fileName);
            using (var fileStream = System.IO.File.Create(path))
            {
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }

            var extension = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal) + 1);
            var ex = extensionService.GetByName(extension);
            if (ex == null)
            {
                extensionService.Create(new ExtensionEntity()
                {
                    Name = extension
                });
            }
            imageService.Create(new ImageEntity()
            {
                Name = fileName,
                Description = description,
                ExtensionId = extensionService.GetByName(extension).Id,
                AlbumId = albumId,
                IsTradable = isTradable
            });

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        private string GetPathToImg(string fileName)
        {
            var serverPath = Server.MapPath("~");
            return Path.Combine(serverPath, "Content", "img", fileName);
        }

        private Image BuildImage(string path)
        {
            var fileName = Path.GetFileName(path);
            var image = new Image
            {
                Url = Url.Content("~/Content/img/" + fileName),
                Name = Path.GetFileNameWithoutExtension(path)
            };

            return image;
        }
    }
}
