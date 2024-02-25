using Chinook.Domain.Converters;
using Chinook.Domain.Entities;

namespace Chinook.Domain.ApiModels;

public sealed class TrackApiModel : BaseApiModel, IConvertModel<Track>
{
    public string Name { get; set; } = null!;

    public int? AlbumId { get; set; }

    public int? MediaTypeId { get; set; }

    public int? GenreId { get; set; }

    public string? Composer { get; set; }

    public int Milliseconds { get; set; }

    public int? Bytes { get; set; }

    public decimal UnitPrice { get; set; }

    public AlbumApiModel? Album { get; set; }

    public GenreApiModel? Genre { get; set; }

    public ICollection<InvoiceLineApiModel> InvoiceLines { get; set; } = new List<InvoiceLineApiModel>();

    public MediaTypeApiModel? MediaType { get; set; } = null!;

    public ICollection<PlaylistApiModel> Playlists { get; set; } = new List<PlaylistApiModel>();
    public string AlbumName { get; set; }
    public string? MediaTypeName { get; set; }
    public string? GenreName { get; set; }

    public Track Convert() =>
        new()
        {
            Id = Id,
            Name = Name,
            AlbumId = AlbumId,
            MediaTypeId = MediaTypeId,
            GenreId = GenreId,
            Composer = Composer,
            Milliseconds = Milliseconds,
            Bytes = Bytes,
            UnitPrice = UnitPrice
        };
}