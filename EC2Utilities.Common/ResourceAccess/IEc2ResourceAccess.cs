using System.Collections.Generic;
using Amazon.EC2.Model;

namespace EC2Utilities.Common.ResourceAccess
{
    public interface IEc2ResourceAccess : IEc2UtilitiesResourceAccess
    {
        DescribeInstancesResult GetInstances(Ec2Key ec2Key);

        DescribeVolumesResult GetVolumes(Ec2Key ec2Key);

        Snapshot SnapshotVolume(Ec2Key ec2Key, string volumeId, string snapshotDescription, string backupType);

        List<Snapshot> GetSnapshots(Ec2Key ec2Key);

        void DeleteSnapshot(Ec2Key ec2Key, string snapshotId);

        void StartUpInstance(Ec2Key ec2Key, string instanceId);

        void AssociateIpToInstance(Ec2Key ec2Key, string instanceId, string ip);

        void SendEmail(Ec2Key ec2Key, string fromAddress, List<string> toAddresses, string subject, string body);
    }
}
