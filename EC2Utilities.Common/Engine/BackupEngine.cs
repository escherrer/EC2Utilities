using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.EC2.Model;
using EC2Utilities.Common.ResourceAccess;
using NLog;

namespace EC2Utilities.Common.Engine
{
    public class BackupEngine : IBackupEngine
    {
        private readonly IConfigResourceAccess _configResourceAccess;
        private readonly IEc2ResourceAccess _ec2ResourceAccess;
        private readonly Logger _logger;

        public BackupEngine(IConfigResourceAccess configResourceAccess, IEc2ResourceAccess ec2ResourceAccess, Logger logger)
        {
            _configResourceAccess = configResourceAccess;
            _ec2ResourceAccess = ec2ResourceAccess;
            _logger = logger;
        }

        public void BackupInstances()
        {
            Ec2Key ec2Key = _configResourceAccess.GetEc2Key();
            DescribeVolumesResult eC2Volumes = _ec2ResourceAccess.GetVolumes(ec2Key);
            DescribeInstancesResult ec2Instances = _ec2ResourceAccess.GetInstances(ec2Key);

            foreach (Volume eC2Volume in eC2Volumes.Volume)
            {
                IEnumerable<string> instanceNames;

                if (eC2Volume.IsSetAttachment())
                {
                    instanceNames = eC2Volume.Attachment.Select(x => GetInstanceName(x.InstanceId, ec2Instances.Reservation));
                }
                else
                {
                    instanceNames = new [] { "(Unattached)" };
                }

                string volumeName = GetVolumeName(eC2Volume);
                string instanceName = string.Join(",", instanceNames);

                _logger.Info("Sending back up of volume {0} of instance {1} request.", volumeName, instanceName);

                string snapshotDescription = string.Format("{0} {1} Backup", instanceName, volumeName);
                _ec2ResourceAccess.SnapshotVolume(ec2Key, eC2Volume.VolumeId, snapshotDescription, "Automatic");

                _logger.Info("Back up of volume {0} of instance {1} request sent.", volumeName, instanceName);
            }
        }

        public void PurgeBackups(int backupRetentionDays)
        {
            Ec2Key ec2Key = _configResourceAccess.GetEc2Key();

            var snapshots = _ec2ResourceAccess.GetSnapshots(ec2Key);

            foreach (var snapshot in snapshots)
            {
                if (snapshot.Tag.Any(x => x.Key == "EC2BackupSnapshotType" && x.Value == "Automatic"))
                {
                    DateTime backupDate = DateTime.Parse(snapshot.StartTime);

                    if (backupDate.AddDays(backupRetentionDays) < DateTime.Now)
                    {
                        _logger.Info("Sending request to delete snapshotId {0}.", snapshot.SnapshotId);
                        _ec2ResourceAccess.DeleteSnapshot(ec2Key, snapshot.SnapshotId);
                        _logger.Info("Request to delete snapshotId {0} sent.", snapshot.SnapshotId);
                    }
                }
            }
        }

        private static string GetInstanceName(string instanceId, IEnumerable<Reservation> reservations)
        {
            foreach (Reservation reservation in reservations)
            {
                foreach (RunningInstance runningInstance in reservation.RunningInstance)
                {                   
                    if (runningInstance.InstanceId == instanceId)
                    {
                        var nameTag = runningInstance.Tag.FirstOrDefault(x => x.Key == "Name");
                        
                        if (null != nameTag && !string.IsNullOrWhiteSpace(nameTag.Value))
                        {
                            return nameTag.Value;
                        }
                    }
                }
            }
            return instanceId;
        }

        private static string GetVolumeName(Volume volume)
        {
            var nameTag = volume.Tag.FirstOrDefault(x => x.Key == "Name");

            if (null != nameTag && !string.IsNullOrWhiteSpace(nameTag.Value))
            {
                return nameTag.Value;
            }

            return volume.VolumeId;
        }
    }
}
