namespace Aspect;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class AspectAttribute: Attribute
{
    public virtual void OnMethodCall(object targetInstance) {}
    public virtual void OnMethodExit(object targetInstance, object? returnValue) {}
}
