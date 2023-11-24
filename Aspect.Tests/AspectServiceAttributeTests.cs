namespace Aspect.Tests;

public class AspectServiceAttributeTests
{
    public AspectServiceAttributeTests()
    {
        AspectProxy.ServiceFactory = new DefaultServiceFactory();
    }

    public class Dummy: IDummy
    {
        public int BeforeInvoked;
        public int AfterInvoked;
        public int ReturnValue;

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
        public void OnMethodCall(object targetInstance)
        {
            var dummy = (Dummy)targetInstance;
            dummy.BeforeInvoked += 1;
        }

        public void OnMethodExit(object targetInstance, object? returnValue)
        {
            var dummy = (Dummy)targetInstance;
            dummy.AfterInvoked += 1;
            if(returnValue != null)
            {
                dummy.ReturnValue = (int)returnValue;
            }
        }
    }

    [Fact]
    public void The_service_type_is_resolved_correctly()
    {
        var dummy = new Dummy();
        var proxy = AspectProxy.Create<IDummy>(dummy);
        proxy.Foo();
        Assert.Equal(1, dummy.BeforeInvoked);
        Assert.Equal(1, dummy.AfterInvoked);
        Assert.Equal(42, dummy.ReturnValue);
    }
}