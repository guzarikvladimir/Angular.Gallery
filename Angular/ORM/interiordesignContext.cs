namespace ORM
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class interiordesignContext : DbContext
    {
        public interiordesignContext()
            : base("name=interiordesignContext")
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Extension> Extensions { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
                .HasMany(e => e.Images)
                .WithRequired(e => e.Album)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Extension>()
                .HasMany(e => e.Images)
                .WithRequired(e => e.Extension)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Image>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Albums)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
