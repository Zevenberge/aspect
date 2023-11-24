using System.Diagnostics.CodeAnalysis;

namespace Aspect.Extensions.Hosting;

internal static class ThrowHelper
{
    public static void ThrowIfServiceProviderIsNull([NotNull]IServiceProvider? serviceProvider)
    {
        if(serviceProvider == null)
        {
            throw new InvalidOperationException("Trying to access a scoped service provider that was not set. Configure it with `app.UseScopedAspectServices()`");
        }
    }
}