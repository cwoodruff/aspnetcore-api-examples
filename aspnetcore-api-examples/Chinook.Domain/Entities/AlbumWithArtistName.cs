namespace Chinook.Domain.Entities;

public class AlbumWithArtistName : BaseEntity
{
    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public string? Name { get; set; }
}