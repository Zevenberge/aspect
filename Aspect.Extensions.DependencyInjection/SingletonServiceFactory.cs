using Microsoft.Extensions.DependencyInjection;

namespace Aspect.Extensions.DependencyInjection;

internal class SingletonServiceFactory(IServiceProvider serviceProvider) : IServiceFactory
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public IAspectService Resolve(Type serviceType)
    {
        return (IAspectService)ServiceProvider.GetRequiredService(serviceType);
    }
}