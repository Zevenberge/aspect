namespace Aspect.Tests;

public class AspectAttributeTests
{
    const int FOO_INVOKED = 7;
    const int BAR_INVOKED = 13;
    public class Dummy: IDummy
    {
        public int BeforeInvoked;
        public int AfterInvoked;
        public int ReturnValue;

        [DummyAspect(FOO_INVOKED)]
        public int Foo()
        {
            return 42;
        }

        [DummyAspect(BAR_INVOKED)]
        public void Bar()
        {

        }
    }

    public interface IDummy
    {
        int Foo();
        void Bar();
    }

    public class DummyAspectAttribute(int value) : AspectAttribute
    {
        public int Value { get; } = value;

        public override void OnMethodCall(object targetInstance)
        {
            var dummy = (Dummy)targetInstance;
            dummy.BeforeInvoked += Value;
        }

        public override void OnMethodExit(object targetInstance, object? returnValue)
        {
            var dummy = (Dummy)targetInstance;
            dummy.AfterInvoked += Value;
            if(returnValue != null)
            {
                dummy.ReturnValue = (int)returnValue;
            }
        }
    }

    [Fact]
    public void The_before_is_invoked()
    {
        var dummy = new Dummy();
        var proxy = AspectProxy.Create<IDummy>(dummy);
        proxy.Foo();
        Assert.Equal(FOO_INVOKED, dummy.BeforeInvoked);
    }

    [Fact]
    public void The_after_is_invoked()
    {
        var dummy = new Dummy();
        var proxy = AspectProxy.Create<IDummy>(dummy);
        proxy.Foo();
        Assert.Equal(FOO_INVOKED, dummy.AfterInvoked);
    }

    [Fact]
    public void The_return_value_is_saved()
    {
        var dummy = new Dummy();
        var proxy = AspectProxy.Create<IDummy>(dummy);
        proxy.Foo();
        Assert.Equal(42, dummy.ReturnValue);
    }

    [Fact]
    public void Each_method_has_their_own_attributes()
    {
        var dummy = new Dummy();
        var proxy = AspectProxy.Create<IDummy>(dummy);
        proxy.Bar();
        Assert.Equal(BAR_INVOKED, dummy.BeforeInvoked);
        Assert.Equal(BAR_INVOKED, dummy.AfterInvoked);
    }

    [Fact]
    public void A_void_return_value_is_a_null()
    {
        var dummy = new Dummy();
        var proxy = AspectProxy.Create<IDummy>(dummy);
        proxy.Bar();
        Assert.Equal(0, dummy.ReturnValue);
    }
}
