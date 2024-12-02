using Microsoft.EntityFrameworkCore;
using VinylStore.Api.Persistence.Entities;

namespace VinylStore.Api.Persistence
{
    public class VinylStoreContext : DbContext
    {
        public DbSet<Album> Albums => Set<Album>();
        public DbSet<Track> Tracks => Set<Track>();

        public VinylStoreContext(DbContextOptions<VinylStoreContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AlbumConfig());
            modelBuilder.ApplyConfiguration(new TrackConfig());
        }
    }
}
