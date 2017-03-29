using System;

namespace BLL.Interfaces.Entities
{
    public class ImageEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ExtensionId { get; set; }
        public int AlbumId { get; set; }
        public bool IsTradable { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? IsBought { get; set; }
        public int? UserId { get; set; }
        public decimal? Price { get; set; }


        public AlbumEntity Album { get; set; }
    }
}