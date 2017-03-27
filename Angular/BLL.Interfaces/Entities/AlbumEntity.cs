using System;
using System.Collections.Generic;

namespace BLL.Interfaces.Entities
{
    public class AlbumEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }


        public UserEntity User { get; set; }
        public List<ImageEntity> Images { get; set; }
    }
}