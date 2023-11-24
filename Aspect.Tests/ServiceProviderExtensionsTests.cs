using Aspect.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Aspect.Tests;

public class ServiceProviderExtensionsTests
{
    public class Dummy: IDummy
    {
        public int BeforeInvoked;

        [AspectService(typeof(DummyAspectService))]
        public int Foo()
        {
            return 42;
        }
    }

    public interface IDummy
    {
        int Foo();
    }

    public class DummyAspectService: IAspectService
    {
        public DummyAspectService(DummyLogger logger)
        {
            _ = logger;
        }

        public void OnMethodCall(object targetInstance)
        {
            var dummy = (Dummy)targetInstance;
            dummy.BeforeInvoked += 1;
        }

        public void OnMethodExit(object targetInstance, object? returnValue)
        {
            
        }
    }

    public class DummyLogger { }

    [Fact]
    public void A_registered_aspect_service_is_resolved()
    {
        var dummy = new Dummy();
        var services = new ServiceCollection();
        services.AddSingletonWithAspects<IDummy, Dummy>(dummy);
        services.AddSingleton<DummyAspectService>();
        services.AddSingleton<DummyLogger>();
        var provider = services.BuildServiceProvider();
        provider.UseSingletonAspectServices();
        var dummy1 = provider.GetRequiredService<IDummy>();
        dummy1.Foo();
        Assert.Equal(1, dummy.BeforeInvoked);
    }
}