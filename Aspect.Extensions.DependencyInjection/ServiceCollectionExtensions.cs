using Microsoft.Extensions.DependencyInjection;

namespace Aspect.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public delegate TInterface Factory<TInterface>(IServiceProvider serviceProvider);

     public static IServiceCollection AddSingletonWithAspects<TInterface, TImplementation>(this IServiceCollection services, TImplementation @object)
        where TImplementation : class, TInterface
        where TInterface : class
    {
        services.AddSingleton(AspectProxy.Create<TInterface>(@object));
        return services;
    }

    public static IServiceCollection AddSingletonWithAspects<TInterface, TImplementation>(this IServiceCollection services) 
        where TImplementation : class, TInterface
        where TInterface : class
    {
        var key = Guid.NewGuid();
        services.AddKeyedSingleton<TInterface, TImplementation>(key);
        services.AddSingleton(svp => AspectProxy.Create(svp.GetRequiredKeyedService<TInterface>(key)));
        return services;
    }

    public static IServiceCollection AddSingletonWithAspects<TInterface>(this IServiceCollection services, Factory<TInterface> factory)
        where TInterface : class
    {
        var key = Guid.NewGuid();
        services.AddKeyedSingleton(key, (svp, _) => factory(svp));
        services.AddSingleton(svp => AspectProxy.Create(svp.GetRequiredKeyedService<TInterface>(key)));
        return services;
    }

    public static IServiceCollection AddScopedWithAspects<TInterface, TImplementation>(this IServiceCollection services) 
        where TImplementation : class, TInterface
        where TInterface : class
    {
        var key = Guid.NewGuid();
        services.AddKeyedScoped<TInterface, TImplementation>(key);
        services.AddScoped(svp => AspectProxy.Create(svp.GetRequiredKeyedService<TInterface>(key)));
        return services;
    }

    public static IServiceCollection AddScopedWithAspects<TInterface>(this IServiceCollection services, Factory<TInterface> factory)
        where TInterface : class
    {
        var key = Guid.NewGuid();
        services.AddKeyedScoped(key, (svp, _) => factory(svp));
        services.AddScoped(svp => AspectProxy.Create(svp.GetRequiredKeyedService<TInterface>(key)));
        return services;
    }

    public static IServiceCollection AddTransientWithAspects<TInterface, TImplementation>(this IServiceCollection services) 
        where TImplementation : class, TInterface
        where TInterface : class
    {
        var key = Guid.NewGuid();
        services.AddKeyedTransient<TInterface, TImplementation>(key);
        services.AddTransient(svp => AspectProxy.Create(svp.GetRequiredKeyedService<TInterface>(key)));
        return services;
    }

    public static IServiceCollection AddTransientWithAspects<TInterface>(this IServiceCollection services, Factory<TInterface> factory)
        where TInterface : class
    {
        var key = Guid.NewGuid();
        services.AddKeyedTransient(key, (svp, _) => factory(svp));
        services.AddTransient(svp => AspectProxy.Create(svp.GetRequiredKeyedService<TInterface>(key)));
        return services;
    }
}
