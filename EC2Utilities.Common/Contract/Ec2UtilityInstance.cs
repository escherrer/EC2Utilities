namespace EC2Utilities.Common.Contract
{
    public class Ec2UtilityInstance
    {
        public string InstanceName { get; set; }

        public string InstanceId { get; set; }

        public Ec2UtilityInstanceStatus Status { get; set; }

        public string DefaultIp { get; set; }

        public string ImageId { get; set; }

        public string InstanceType { get; set; }
    }
}
