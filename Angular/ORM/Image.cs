namespace ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int ExtensionId { get; set; }

        public int AlbumId { get; set; }

        public bool? isTradable { get; set; }

        public virtual Album Album { get; set; }

        public virtual Extension Extension { get; set; }
    }
}
