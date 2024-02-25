using Chinook.Domain.ApiModels;
using Chinook.Domain.Converters;

namespace Chinook.Domain.Entities;

public sealed class MediaType : BaseEntity, IConvertModel<MediaTypeApiModel>
{
    public string? Name { get; set; }

    public ICollection<Track> Tracks { get; set; } = new List<Track>();

    public MediaTypeApiModel Convert() =>
        new()
        {
            Id = Id,
            Name = Name
        };
}