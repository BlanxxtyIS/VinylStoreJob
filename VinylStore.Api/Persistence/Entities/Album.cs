using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VinylStore.Api.Persistence.Entities
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string? Image { get; set; } = "";
        public string AuthorName { get; set; } = "";
        public int TimeInMinutes { get; set; }
        public string TimeFormatted => $"{TimeInMinutes / 60}h {TimeInMinutes % 60}m";
        public int Raiting { get; set; }
        public IEnumerable<Track> Tracks { get; set; } = default!;
    }

    public class AlbumConfig : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.AuthorName).IsRequired();
            builder.Property(x => x.Raiting).IsRequired();
        }
    }
}
