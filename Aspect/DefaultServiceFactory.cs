namespace Aspect;

public class DefaultServiceFactory : IServiceFactory
{
    public IAspectService Resolve(Type serviceType)
    {
        ThrowHelper.ThrowIfNotAssignableToIAspectService(serviceType);
        ThrowHelper.ThrowIfNotAbleToActivate(serviceType);
        return (IAspectService)Activator.CreateInstance(serviceType)!;
    }
}
