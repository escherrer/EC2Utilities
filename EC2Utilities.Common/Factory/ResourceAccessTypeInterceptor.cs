using System;
using System.Linq;
using Castle.DynamicProxy;
using EC2Utilities.Common.ResourceAccess;
using StructureMap;
using StructureMap.Interceptors;

namespace EC2Utilities.Common.Factory
{
    public class ResourceAccessTypeInterceptor : TypeInterceptor
    {
        private readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();

        public bool MatchesType(Type type)
        {
            //http://www.hanselman.com/blog/DoesATypeImplementAnInterface.aspx
            if (null != type.GetInterface(typeof (IEc2UtilitiesResourceAccess).FullName))
                return true;

            return false;
        }

        public object Process(object target, IContext context)
        {
            return _proxyGenerator.CreateInterfaceProxyWithTargetInterface(
                target.GetType().GetInterfaces().First(),
                target.GetType().GetInterfaces(),
                target, new ResourceAccessInterceptor());
        }
    }
}