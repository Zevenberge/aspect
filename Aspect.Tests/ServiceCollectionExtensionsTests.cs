using Aspect.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Aspect.Tests;

public class ServiceCollectionExtensionsTests
{
    public class Dummy: IDummy
    {
        public int BeforeInvoked;

        [DummyAspect]
        public int Foo()
        {
            return 42;
        }
    }

    public interface IDummy
    {
        int Foo();
    }

    public class DummyAspectAttribute : AspectAttribute
    {
        public override void OnMethodCall(object targetInstance)
        {
            var dummy = (Dummy)targetInstance;
            dummy.BeforeInvoked += 1;
        }
    }

    [Fact]
    public void A_singleton_object_is_registered_as_a_singleton()
    {
        var dummy = new Dummy();
        var services = new ServiceCollection();
        services.AddSingletonWithAspects<IDummy, Dummy>(dummy);
        var provider = services.BuildServiceProvider();
        IDummy dummy1;
        using(var scope = provider.CreateScope())
        {
            dummy1 = scope.ServiceProvider.GetRequiredService<IDummy>();
        }
        using(var scope = provider.CreateScope())
        {
            var dummy2 = scope.ServiceProvider.GetRequiredService<IDummy>();
            Assert.Same(dummy1, dummy2);
        }
        dummy1.Foo();
        Assert.Equal(1, dummy.BeforeInvoked);
    }

    [Fact]
    public void A_singleton_type_is_registered_as_a_singleton()
    {
        var services = new ServiceCollection();
        services.AddSingletonWithAspects<IDummy, Dummy>();
        var provider = services.BuildServiceProvider();
        IDummy dummy1;
        using(var scope = provider.CreateScope())
        {
            dummy1 = scope.ServiceProvider.GetRequiredService<IDummy>();
        }
        using(var scope = provider.CreateScope())
        {
            var dummy2 = scope.ServiceProvider.GetRequiredService<IDummy>();
            Assert.Same(dummy1, dummy2);
        }
        Assert.IsNotType<Dummy>(dummy1);
    }

    [Fact]
    public void A_singleton_factory_is_registered_as_a_singleton()
    {
        var services = new ServiceCollection();
        services.AddSingletonWithAspects<IDummy>(_ => new Dummy());
        var provider = services.BuildServiceProvider();
        IDummy dummy1;
        using(var scope = provider.CreateScope())
        {
            dummy1 = scope.ServiceProvider.GetRequiredService<IDummy>();
        }
        using(var scope = provider.CreateScope())
        {
            var dummy2 = scope.ServiceProvider.GetRequiredService<IDummy>();
            Assert.Same(dummy1, dummy2);
        }
        Assert.IsNotType<Dummy>(dummy1);
    }

    [Fact]
    public void A_scoped_type_is_registered_as_scoped()
    {
        var services = new ServiceCollection();
        services.AddScopedWithAspects<IDummy, Dummy>();
        var provider = services.BuildServiceProvider();
        IDummy dummy1;
        using(var scope = provider.CreateScope())
        {
            // Don't try this at home
            dummy1 = scope.ServiceProvider.GetRequiredService<IDummy>();
        }
        using(var scope = provider.CreateScope())
        {
            var dummy2 = scope.ServiceProvider.GetRequiredService<IDummy>();
            Assert.NotSame(dummy1, dummy2);
        }
        Assert.IsNotType<Dummy>(dummy1);
    }

    [Fact]
    public void A_scoped_factory_is_registered_as_scoped()
    {
        var services = new ServiceCollection();
        services.AddScopedWithAspects<IDummy>(_ => new Dummy());
        var provider = services.BuildServiceProvider();
        IDummy dummy1;
        using(var scope = provider.CreateScope())
        {
            // Don't try this at home
            dummy1 = scope.ServiceProvider.GetRequiredService<IDummy>();
        }
        using(var scope = provider.CreateScope())
        {
            var dummy2 = scope.ServiceProvider.GetRequiredService<IDummy>();
            Assert.NotSame(dummy1, dummy2);
        }
        Assert.IsNotType<Dummy>(dummy1);
    }

    [Fact]
    public void A_transient_type_is_registered_as_transient()
    {
        var services = new ServiceCollection();
        services.AddTransientWithAspects<IDummy, Dummy>();
        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var dummy1 = scope.ServiceProvider.GetRequiredService<IDummy>();
        var dummy2 = scope.ServiceProvider.GetRequiredService<IDummy>();
        Assert.NotSame(dummy1, dummy2);
        Assert.IsNotType<Dummy>(dummy1);
    }

    [Fact]
    public void A_transient_factory_is_registered_as_transient()
    {
        var services = new ServiceCollection();
        services.AddTransientWithAspects<IDummy>(_ => new Dummy());
        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var dummy1 = scope.ServiceProvider.GetRequiredService<IDummy>();
        var dummy2 = scope.ServiceProvider.GetRequiredService<IDummy>();
        Assert.NotSame(dummy1, dummy2);
        Assert.IsNotType<Dummy>(dummy1);
    }
}
