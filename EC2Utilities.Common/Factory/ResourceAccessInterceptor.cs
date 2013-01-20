using System;
using Castle.DynamicProxy;
using EC2Utilities.Common.Exceptions;
using log4net;

namespace EC2Utilities.Common.Factory
{
    public class ResourceAccessInterceptor : IInterceptor
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ResourceAccessInterceptor));

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception e)
            {
                _logger.Error("An exception has occurred in resource access.", e);
                throw new ResourceAccessException(e);
            }
        }
    }
}