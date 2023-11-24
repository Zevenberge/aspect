namespace Aspect;

public sealed class AspectServiceAttribute : AspectAttribute
{
    public Type ServiceType { get; }

    public AspectServiceAttribute(Type serviceType)
    {
        ThrowHelper.ThrowIfNotAssignableToIAspectService(serviceType);
        ServiceType = serviceType;
    }

    public override void OnMethodCall(object targetInstance)
    {
        AspectProxy.ServiceFactory.Resolve(ServiceType)?.OnMethodCall(targetInstance);
    }

    public override void OnMethodExit(object targetInstance, object? returnValue)
    {
        AspectProxy.ServiceFactory.Resolve(ServiceType)?.OnMethodExit(targetInstance, returnValue);
    }
}
