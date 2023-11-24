namespace Aspect;

public interface IServiceFactory
{
    IAspectService Resolve(Type serviceType);
}
