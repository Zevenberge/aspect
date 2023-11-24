namespace Aspect;

public interface IAspectService
{
    void OnMethodCall(object targetInstance);
    void OnMethodExit(object targetInstance, object? returnValue);
}