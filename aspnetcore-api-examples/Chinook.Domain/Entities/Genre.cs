using Chinook.Domain.ApiModels;
using Chinook.Domain.Converters;

namespace Chinook.Domain.Entities;

public sealed class Genre : BaseEntity, IConvertModel<GenreApiModel>
{
    public string? Name { get; set; }

    public ICollection<Track> Tracks { get; set; } = new List<Track>();

    public GenreApiModel Convert() =>
        new()
        {
            Id = Id,
            Name = Name
        };
}