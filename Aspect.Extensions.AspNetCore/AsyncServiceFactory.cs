using Microsoft.Extensions.DependencyInjection;

namespace Aspect.Extensions.Hosting;

internal class AsyncServiceFactory : IServiceFactory
{
    private sealed class Holder
    {
        public IServiceProvider? ServiceProvider;
    }
    private AsyncLocal<Holder> _currentScope = new();

    public IServiceProvider? CurrentServiceProvider
    {
        get => _currentScope.Value?.ServiceProvider;
        set
        {
            var holder = _currentScope.Value;
            if(holder != null)
            {
                holder.ServiceProvider = null;
            }
            if(value != null)
            {
                _currentScope.Value = new Holder { ServiceProvider = value };
            }
        }
    }

    public IAspectService Resolve(Type serviceType)
    {
        var currentServiceProvider = CurrentServiceProvider;
        ThrowHelper.ThrowIfServiceProviderIsNull(currentServiceProvider);
        return (IAspectService)currentServiceProvider.GetRequiredService(serviceType);
    }
}
