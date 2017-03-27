using System.Collections.Generic;

namespace BLL.Interfaces.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }


        public RoleEntity Role { get; set; }
        public List<AlbumEntity> Albums { get; set; }
    }
}