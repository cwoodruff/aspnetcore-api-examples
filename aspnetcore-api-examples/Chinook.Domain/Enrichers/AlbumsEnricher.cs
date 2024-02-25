using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class AlbumsEnricher(AlbumEnricher enricher) : ListEnricher<List<AlbumApiModel>>
{
    public override async Task Process(List<object> representations)
    {
        foreach (var album in representations)
        {
            await enricher.Process(album as AlbumApiModel);
        }
    }
}