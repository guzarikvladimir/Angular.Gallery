using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Angular.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int AlbumId { get; set; }
        public int ExtensionId { get; set; }
        public double? CreationDate { get; set; }
        public bool? IsBought { get; set; }
        public int? UserId { get; set; }
        public decimal? Price { get; set; }
        public bool? IsTradable { get; set; }
    }
}