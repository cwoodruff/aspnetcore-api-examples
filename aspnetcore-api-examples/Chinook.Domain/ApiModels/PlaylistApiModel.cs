using Chinook.Domain.Converters;
using Chinook.Domain.Entities;

namespace Chinook.Domain.ApiModels;

public sealed class PlaylistApiModel : BaseApiModel, IConvertModel<Playlist>
{
    public string? Name { get; set; }

    public ICollection<TrackApiModel> Tracks { get; set; } = new List<TrackApiModel>();

    public Playlist Convert() =>
        new()
        {
            Id = Id,
            Name = Name
        };
}