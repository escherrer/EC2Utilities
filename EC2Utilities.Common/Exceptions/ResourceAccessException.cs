using System;

namespace EC2Utilities.Common.Exceptions
{
    public class ResourceAccessException : Exception
    {
        public ResourceAccessException(Exception e) : base("A resource access exception has occurred. See inner exception for details.", e)
        { }
    }

    public class InvalidInstanceTypeException : Exception
    {
        public InvalidInstanceTypeException(string message)
            : base(message)
        { }
    }
}
