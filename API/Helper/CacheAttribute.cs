using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Services.Cache_Service;
using System.Text;

namespace API.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CacheAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // getting an instance of the `ICacheService` service from the dependency injection container 
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // trying to find a cached response with key matches the cache key generated
            var cachedResponse = await cacheService.GetCacheResponseAsync(cacheKey);

            // if a cached response is found:
            if (!string.IsNullOrEmpty (cachedResponse))
            {
                // setting ContentResult properties to match the cached response
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                // return the cached response for the current request to the client without executing the endpoint
                context.Result = contentResult;
                return;
            }

            // In case there is no cached response for the current request, invoke the next action in the pipeline (the endpoint)
            var executedContext = await next();

            // Cache that response for the future requests, if the executed action result is of type `OkObjectResult` object
            if (executedContext.Result is OkObjectResult response)
                await cacheService.SetCacheResponseAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
        }

        private string GenerateCacheKeyFromRequest (HttpRequest request) 
        {
            var cacheKey = new StringBuilder();

            cacheKey.Append($"{request.Path}");

            foreach(var (key,value) in request.Query.OrderBy(x => x.Key))
                cacheKey.Append($"|{key}-{value}");

            return cacheKey.ToString();
        }
    }
}
