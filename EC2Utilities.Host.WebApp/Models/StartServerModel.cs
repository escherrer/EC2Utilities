using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EC2Utilities.Common.Contract;

namespace EC2Utilities.Host.WebApp.Models
{
    public class StartServerModel
    {
        public StartServerModel()
        { }

        public StartServerModel(Ec2UtilityInstance ec2UtilityInstance)
        {
            ServerId = ec2UtilityInstance.InstanceId;
            ServerName = ec2UtilityInstance.InstanceName;
            ServerStatus = ec2UtilityInstance.Status.ToString();
            ServerType = ec2UtilityInstance.InstanceType;
            AvailableServerTypes = new List<string>();
        }

        public string ServerName { get; set; }

        public string ServerId { get; set; }

        public string ServerStatus { get; set; }

        public string ServerType { get; set; }

        public List<string> AvailableServerTypes { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    }
}
