using Chinook.Domain.ApiModels;
using Chinook.Domain.Converters;

namespace Chinook.Domain.Entities;

public sealed class Album : BaseEntity, IConvertModel<AlbumApiModel>
{
    public string Title { get; set; } = null!;

    public int? ArtistId { get; set; }

    public Artist Artist { get; set; } = null!;

    public ICollection<Track> Tracks { get; set; } = new List<Track>();

    public AlbumApiModel Convert() =>
        new()
        {
            Id = Id,
            ArtistId = ArtistId,
            Title = Title
        };
}