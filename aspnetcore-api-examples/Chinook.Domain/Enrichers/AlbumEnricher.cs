using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class AlbumEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<AlbumApiModel>
{
    public override Task Process(AlbumApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
            httpContext!,
            "album",
            new { id = representation!.Id },
            scheme: "https"
        );

        representation.AddLink(new Link
        {
            Id = representation.Id.ToString(),
            Label = $"Album: {representation.Title} #{representation.Id}",
            Url = url!
        });

        return Task.CompletedTask;
    }
}