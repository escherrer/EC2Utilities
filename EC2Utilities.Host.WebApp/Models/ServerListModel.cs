using EC2Utilities.Common.Contract;

namespace EC2Utilities.Host.WebApp.Models
{
    public class ServerListModel
    {
        public ServerListModel()
        { }

        public ServerListModel(Ec2UtilityInstance ec2UtilityInstance)
        {
            ServerId = ec2UtilityInstance.InstanceId;
            ServerName = ec2UtilityInstance.InstanceName;
            ServerType = ec2UtilityInstance.InstanceType;
            ServerStatus = ec2UtilityInstance.Status.ToString();
        }

        public string ServerName { get; set; }

        public string ServerId { get; set; }

        public string ServerStatus { get; set; }

        public string ServerType { get; set; }
    }
}