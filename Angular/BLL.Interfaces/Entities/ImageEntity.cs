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

        public AlbumEntity Album { get; set; }
    }
}