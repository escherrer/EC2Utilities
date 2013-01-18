using System;
using Castle.DynamicProxy;
using EC2Utilities.Common.Exceptions;

namespace EC2Utilities.Common.Factory
{
    public class ResourceAccessInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                throw new ResourceAccessException(e);
            }
        }
    }
}