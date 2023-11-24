namespace Aspect;

internal static class ThrowHelper
{
    public static void ThrowIfNotAssignableToIAspectService(Type serviceType)
    {
        if(!serviceType.IsAssignableTo(typeof(IAspectService)))
        {
            throw new ArgumentException(nameof(serviceType), $"{serviceType.FullName} is not assignable to {nameof(IAspectService)}");
        }
    }

    public static void ThrowIfNotAbleToActivate(Type serviceType)
    {
        if(!serviceType.IsClass || serviceType.IsAbstract)
        {
            throw new ArgumentException(nameof(serviceType), $"Cannot create an instance of abstract class or interface {serviceType.FullName}");
        }
        if(!serviceType.GetConstructors().Any(c => c.IsPublic && c.GetParameters().Length == 0))
        {
            throw new ArgumentException(nameof(serviceType), $"{serviceType.FullName} does not contain a parameterless public constructor");
        }
    }
}