namespace EC2Utilities.Common.Contract
{
    public class Ec2UtilityInstance
    {
        public string InstanceName { get; set; }

        public string InstanceId { get; set; }

        public Ec2UtilityInstanceStatus Status { get; set; }

        public string DefaultIp { get; set; }
    }
}
