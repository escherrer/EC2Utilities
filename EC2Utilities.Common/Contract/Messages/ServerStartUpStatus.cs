namespace EC2Utilities.Common.Contract.Messages
{
    public enum ServerStartUpStatus
    {
        Initialized,
        ReSizing,
        Starting,
        Started,
        IpAssigned,
        Complete,
        Unknown
    }
}