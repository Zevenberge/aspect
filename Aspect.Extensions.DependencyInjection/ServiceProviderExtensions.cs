
namespace Aspect.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static IServiceProvider UseSingletonAspectServices(this IServiceProvider serviceProvider)
    {
        AspectProxy.ServiceFactory = new SingletonServiceFactory(serviceProvider);
        return serviceProvider;
    }
}
