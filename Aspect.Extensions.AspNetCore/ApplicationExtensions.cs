using Microsoft.AspNetCore.Builder;

namespace Aspect.Extensions.Hosting;

public static class ApplicationExtensions
{
    public static IApplicationBuilder UseScopedAspectServices(this IApplicationBuilder builder)
    {
        AspectProxy.ServiceFactory = new AsyncServiceFactory();
        builder.Use(async (context, next) => {
            ((AsyncServiceFactory)AspectProxy.ServiceFactory).CurrentServiceProvider = context.RequestServices;
            try
            {
                await next();
            }
            finally
            {
                ((AsyncServiceFactory)AspectProxy.ServiceFactory).CurrentServiceProvider = null;
            }
        });
        return builder;
    }
}
