using Castle.DynamicProxy;

namespace Aspect;

internal class AspectInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var aspects = invocation.MethodInvocationTarget.GetCustomAttributes(inherit: true).OfType<AspectAttribute>().ToList();
        OnMethodCall(aspects, invocation.InvocationTarget);
        invocation.Proceed();
        OnMethodExit(aspects, invocation.InvocationTarget, invocation.ReturnValue);
    }

    private void OnMethodCall(ICollection<AspectAttribute> aspects, object target)
    {
        foreach(var aspect in aspects)
        {
            aspect.OnMethodCall(target);
        }
    }

    private void OnMethodExit(ICollection<AspectAttribute> aspects, object target, object? result)
    {
        foreach(var aspect in aspects)
        {
            aspect.OnMethodExit(target, result);
        }
    }
}