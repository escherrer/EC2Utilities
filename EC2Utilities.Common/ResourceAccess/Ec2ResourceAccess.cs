using System.Collections.Generic;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using NLog;

namespace EC2Utilities.Common.ResourceAccess
{
    public class Ec2ResourceAccess : IEc2ResourceAccess
    {
        private readonly Logger _logger;

        public Ec2ResourceAccess(Logger logger)
        {
            _logger = logger;
        }

        public DescribeInstancesResult GetInstances(Ec2Key ec2Key)
        {
            _logger.Trace("GetInstances Start.");

            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var ec2Request = new DescribeInstancesRequest();

            DescribeInstancesResponse describeInstancesResponse = ec2.DescribeInstances(ec2Request);

            DescribeInstancesResult result = describeInstancesResponse.DescribeInstancesResult;

            _logger.Trace("GetInstances End.");

            return result;
        }

        public DescribeVolumesResult GetVolumes(Ec2Key ec2Key)
        {
            _logger.Trace("GetVolumes Start.");

            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var ec2Request = new DescribeVolumesRequest();

            DescribeVolumesResponse describeVolumesResponse = ec2.DescribeVolumes(ec2Request);

            DescribeVolumesResult result = describeVolumesResponse.DescribeVolumesResult;

            _logger.Trace("GetVolumes End.");

            return result;
        }

        public Snapshot SnapshotVolume(Ec2Key ec2Key, string volumeId, string snapshotDescription, string backupType)
        {
            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var ec2Request = new CreateSnapshotRequest {Description = snapshotDescription, VolumeId = volumeId};

            CreateSnapshotResponse result = ec2.CreateSnapshot(ec2Request);

            var request = new CreateTagsRequest();
            request.ResourceId = new List<string>
            {
                result.CreateSnapshotResult.Snapshot.SnapshotId
            };

            request.Tag = new List<Tag>
                              {
                                  new Tag
                                      {
                                         Key = "Name",
                                         Value = "EC2BackupUtility"
                                      },
                                  new Tag
                                      {
                                         Key = "EC2BackupSnapshotType",
                                         Value = backupType
                                      }
                              };

            ec2.CreateTags(request);

            return result.CreateSnapshotResult.Snapshot;
        }

        public List<Snapshot> GetSnapshots(Ec2Key ec2Key)
        {
            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var ec2Request = new DescribeSnapshotsRequest();

            DescribeSnapshotsResponse result = ec2.DescribeSnapshots(ec2Request);

            return result.DescribeSnapshotsResult.Snapshot;
        }

        public void DeleteSnapshot(Ec2Key ec2Key, string snapshotId)
        {
            _logger.Trace("DeleteSnapshot Start.");

            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var request = new DeleteSnapshotRequest {SnapshotId = snapshotId};

            ec2.DeleteSnapshot(request);

            _logger.Trace("DeleteSnapshot End.");
        }

        public void StartUpInstance(Ec2Key ec2Key, string instanceId)
        {
            _logger.Trace("DeleteSnapshot Start.");

            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var request = new StartInstancesRequest
                              {
                                  InstanceId = new List<string>
                                                   {
                                                       instanceId
                                                   }
                              };

            ec2.StartInstances(request);

            _logger.Trace("StartUpInstance End.");
        }

        public void AssociateIpToInstance(Ec2Key ec2Key, string instanceId, string ip)
        {
            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var request = new AssociateAddressRequest
            {
                InstanceId = instanceId,
                PublicIp = ip
            };

            ec2.AssociateAddress(request);
        }

        public void SendEmail(Ec2Key ec2Key, string fromAddress, List<string> toAddresses, string subject, string body)
        {
            AmazonSimpleEmailService ses = CreateAmazonSes(ec2Key);

            var request = new SendEmailRequest
                              {
                                  Destination = new Destination(toAddresses),
                                  ReplyToAddresses = new List<string> { fromAddress },
                                  Message = new Message
                                                {
                                                    Subject = new Content(subject),
                                                    Body = new Body(new Content(body))
                                                },
                                ReturnPath = fromAddress,
                                Source = fromAddress
                              };

            ses.SendEmail(request);
        }

        private AmazonEC2 CreateAmazonEc2Client(Ec2Key ec2Key)
        {
            var er2Config = new AmazonEC2Config();
            AmazonEC2 ec2 = AWSClientFactory.CreateAmazonEC2Client(ec2Key.AwsAccessKey, ec2Key.AwsSecretKey, er2Config);

            return ec2;
        }

        private AmazonSimpleEmailService CreateAmazonSes(Ec2Key ec2Key)
        {
            var sesConfig = new AmazonSimpleEmailServiceConfig();
            AmazonSimpleEmailService ses = AWSClientFactory.CreateAmazonSimpleEmailServiceClient(ec2Key.AwsAccessKey, ec2Key.AwsSecretKey, sesConfig);

            return ses;
        }
    }
}