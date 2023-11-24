using Castle.DynamicProxy;

namespace Aspect;

public static class AspectProxy
{
    public static T Create<T>(T @object) where T: class
    {
        if(!typeof(T).IsInterface) throw new InvalidOperationException();
        var proxyGenerator = new ProxyGenerator();
        return proxyGenerator.CreateInterfaceProxyWithTarget<T>(@object, new AspectInterceptor());
    }
}
