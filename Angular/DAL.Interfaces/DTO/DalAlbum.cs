using System;
using System.Collections.Generic;

namespace DAL.Interfaces.DTO
{
    public class DalAlbum : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public  DateTime CreationDate { get; set; }
        public int UserId { get; set; }


        public DalUser User { get; set; }
        public List<DalImage> Images { get; set; }
    }
}