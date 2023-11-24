namespace Aspect;

internal static class ThrowHelper
{
    public static void ThrowIfNotAssignableToIAspectService(Type serviceType)
    {
        if(!serviceType.IsAssignableTo(typeof(IAspectService)))
        {
            throw new ArgumentException($"{serviceType.FullName} is not assignable to {nameof(IAspectService)}", nameof(serviceType));
        }
    }

    public static void ThrowIfNotAbleToActivate(Type serviceType)
    {
        if(!serviceType.IsClass || serviceType.IsAbstract)
        {
            throw new ArgumentException($"Cannot create an instance of abstract class or interface {serviceType.FullName}", nameof(serviceType));
        }
        if(!serviceType.GetConstructors().Any(c => c.IsPublic && c.GetParameters().Length == 0))
        {
            throw new ArgumentException($"{serviceType.FullName} does not contain a parameterless public constructor", nameof(serviceType));
        }
    }
}