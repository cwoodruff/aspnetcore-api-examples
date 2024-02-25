using Chinook.Domain.Converters;
using Chinook.Domain.Entities;

namespace Chinook.Domain.ApiModels;

public sealed class GenreApiModel : BaseApiModel, IConvertModel<Genre>
{
    public string? Name { get; set; }

    public ICollection<TrackApiModel> Tracks { get; set; } = new List<TrackApiModel>();

    public Genre Convert() =>
        new()
        {
            Id = Id,
            Name = Name
        };
}