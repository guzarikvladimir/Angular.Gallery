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
using Newtonsoft.Json;

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

        public JsonResult AddToCart(int imageId, int userId)
        {
            var image = imageService.GetById(imageId);
            image.UserId = userId;
            image.IsBought = true;
            imageService.Update(image);
            return Json(true);
        }

        public JsonResult RemoveFromCart(int imageId)
        {
            var image = imageService.GetById(imageId);
            image.UserId = null;
            image.IsBought = false;
            imageService.Update(image);
            return Json(true);
        }

        public JsonResult GetCart(int userId)
        {
            var images = imageService.GetAll().Where(img => img.UserId == userId);
            return Json(images.Select(image => new Image()
            {
                Id = image.Id,
                Name = image.Name,
                Description = image.Description,
                AlbumId = image.AlbumId,
                ExtensionId = image.ExtensionId,
                Url = Path.Combine("\\Content", "img", image.Name),
                CreationDate = image.CreationDate?.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds,
                IsBought = image.IsBought,
                UserId = image.UserId,
                Price = image.Price,
                IsTradable = image.IsTradable
            }), JsonRequestBehavior.AllowGet);
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
                Url = Path.Combine("\\Content", "img", image.Name),
                CreationDate = image.CreationDate?.ToUniversalTime().Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalMilliseconds,
                IsBought = image.IsBought,
                UserId = image.UserId,
                Price = image.Price,
                IsTradable = image.IsTradable
            }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFileAjax(int id)
        {
            var image = imageService.GetById(id);
            var path = GetPathToImg(image.Name);
            System.IO.File.Delete(path);
            imageService.Delete(image);
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
                Url = Path.Combine("\\Content", "img", image.Name),
                CreationDate = image.CreationDate?.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds,
                IsBought = image.IsBought,
                UserId = image.UserId,
                Price = image.Price,
                IsTradable = image.IsTradable
            }));
        }

        public JsonResult AddImageAjax(string fileName, string data, string description, int albumId, bool isTradable, int? price)
        {
            int index = fileName.LastIndexOf(".", StringComparison.Ordinal);
            string extension;
            if (index != -1)
            {
                extension = fileName.Substring(index + 1);
            }
            else
            {
                fileName += ".jpeg";
                extension = "jpeg";
            }

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
                IsTradable = isTradable,
                CreationDate = DateTime.Now,
                IsBought = false,
                UserId = null,
                Price = price
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

        //public JsonResult GetDescription()
        //{
        //    var path = Path.Combine(Server.MapPath("~"), "Content", "json", "description.json");
        //    var description = new { text = "" };
        //    string json;
        //    using (StreamReader file = System.IO.File.OpenText(path))
        //    {
        //        json = file.ReadToEnd();
        //    }
        //    JsonConvert.DeserializeAnonymousType(json, description);
        //    return Json(json);
        //}
    }
}
