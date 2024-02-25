using Chinook.Domain.Converters;
using Chinook.Domain.Entities;

namespace Chinook.Domain.ApiModels;

public sealed class AlbumApiModel : BaseApiModel, IConvertModel<Album>
{
    public string Title { get; set; } = null!;
    public string? ArtistName { get; set; }
    public int? ArtistId { get; set; }
    public ArtistApiModel Artist { get; set; } = null!;
    public ICollection<TrackApiModel> Tracks { get; set; } = new List<TrackApiModel>();

    public Album Convert() =>
        new()
        {
            Id = Id,
            ArtistId = ArtistId,
            Title = Title ?? string.Empty
        };
}