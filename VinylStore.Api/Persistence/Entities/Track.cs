using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VinylStore.Api.Persistence.Entities
{
    public class Track
    {
        public int TrackId { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Album Album { get; set; } = default!;
    }

    public class TrackConfig : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.Property(x => x.AlbumId).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Description).IsRequired();
        }
    }
}
