using System.Collections.Generic;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using log4net;

namespace EC2Utilities.Common.ResourceAccess
{
    public class Ec2ResourceAccess : IEc2ResourceAccess
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(Ec2ResourceAccess));

        public DescribeInstancesResult GetInstances(Ec2Key ec2Key)
        {
            _logger.Debug("GetInstances Start.");

            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var ec2Request = new DescribeInstancesRequest();

            DescribeInstancesResponse describeInstancesResponse = ec2.DescribeInstances(ec2Request);

            DescribeInstancesResult result = describeInstancesResponse.DescribeInstancesResult;

            _logger.Debug("GetInstances End.");

            return result;
        }

        public DescribeVolumesResult GetVolumes(Ec2Key ec2Key)
        {
            _logger.Debug("GetVolumes Start.");

            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var ec2Request = new DescribeVolumesRequest();

            DescribeVolumesResponse describeVolumesResponse = ec2.DescribeVolumes(ec2Request);

            DescribeVolumesResult result = describeVolumesResponse.DescribeVolumesResult;

            _logger.Debug("GetVolumes End.");

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
            _logger.Debug("DeleteSnapshot Start.");

            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var request = new DeleteSnapshotRequest {SnapshotId = snapshotId};

            ec2.DeleteSnapshot(request);

            _logger.Debug("DeleteSnapshot End.");
        }

        public void StartUpInstance(Ec2Key ec2Key, string instanceId)
        {
            _logger.Debug("DeleteSnapshot Start.");

            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var request = new StartInstancesRequest
                              {
                                  InstanceId = new List<string>
                                                   {
                                                       instanceId
                                                   }
                              };

            ec2.StartInstances(request);

            _logger.Debug("StartUpInstance End.");
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

        public List<string> GetImageSizes(Ec2Key ec2Key, string imageId)
        {
            //AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            //var request = new DescribeImagesRequest {ImageId = new List<string> {imageId}};

            //DescribeImagesResponse describeImagesResponse = ec2.DescribeImages(request);

            //
            // According to my research there is currently no way to get the available sizes for a given instance. (boooo!)
            //

            var instanceTypes = new List<string>
                                    {
                                        "t1.micro",
                                        "m1.small",
                                        "m1.medium",
                                        "m1.large",
                                        "m1.xlarge",
                                        "m3.xlarge",
                                        "m3.2xlarge",
                                        "m2.xlarge",
                                        "m2.2xlarge",
                                        "m2.4xlarge",
                                        "c1.medium",
                                        "c1.xlarge",
                                        "cc1.4xlarge",
                                        "cc2.8xlarge",
                                        "cg1.4xlarge",
                                        "hi1.4xlarge",
                                        "hs1.8xlarge"
                                    };

            return instanceTypes;
        }

        public void ModifyInstanceType(Ec2Key ec2Key, string instanceId, string instanceType)
        {
            AmazonEC2 ec2 = CreateAmazonEc2Client(ec2Key);

            var request = new ModifyInstanceAttributeRequest
            {
                Attribute = "instanceType",
                InstanceId = instanceId,
                Value = instanceType
            };

            ec2.ModifyInstanceAttribute(request);
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