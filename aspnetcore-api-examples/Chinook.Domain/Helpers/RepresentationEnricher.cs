using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chinook.Domain.Helpers;

public class RepresentationEnricher(IEnumerable<IEnricher> enrichers) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult result)
        {
            var value = result.Value;
            foreach (var enricher in enrichers)
            {
                if (value != null && await enricher.Match(value))
                {
                    await enricher.Process(value);
                }
            }
        }

        // call this or everything is blank!
        await next();
    }
}