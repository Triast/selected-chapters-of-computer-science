using Microsoft.AspNetCore.Builder;

namespace StateCache.RequestPipeline.Middleware
{
    public static class CachingLastTuplesExtension
    {
        public static IApplicationBuilder UseCachingLastTuples(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CachingLastTuples>();
        }
    }
}
