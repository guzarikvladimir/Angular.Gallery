using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;

namespace Angular.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;

        public AlbumController(IAlbumService albumService, IUserService userService)
        {
            this.albumService = albumService;
            this.userService = userService;
        }

        public JsonResult CreateAlbum(string albumName, string userEmail)
        {
            var user = userService.GetByEmail(userEmail);
            albumService.Create(new AlbumEntity()
            {
                Name = albumName,
                UserId = user.Id,
                CreationDate = DateTime.Now
            });

            return Json(true);
        }

        public JsonResult GetAlbums(int id)
        {
            var albums = albumService.GetByUserId(id);
            ArrayList list = new ArrayList
            {
                new {id = int.MaxValue, name = "--Show All--"}
            };
            foreach (var album in albums)
            {
                list.Add(new { id = album.Id, name = album.Name });
            }
            return Json(list);
        }
    }
}