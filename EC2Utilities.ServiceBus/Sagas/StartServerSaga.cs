using System;
using System.Text;
using EC2Utilities.Common.Contract;
using EC2Utilities.Common.Contract.Messages;
using EC2Utilities.Common.Exceptions;
using EC2Utilities.Common.Manager;
using NServiceBus.Saga;
using StructureMap;

namespace EC2Utilities.ServiceBus.Sagas
{
    public class StartServerSaga : Saga<StartServerSagaData>, IAmStartedByMessages<StartServerCommand>, IHandleTimeouts<CheckServerStatusCommand>
    {
        private readonly IInstanceManager _instanceManager;

        public StartServerSaga()
        {
            _instanceManager = ObjectFactory.GetInstance<IInstanceManager>();
        }
        
        public void Handle(StartServerCommand message)
        {
            Data.Instance = _instanceManager.GetInstance(message.InstanceId);
            Data.NotificationEmailAddress = message.NotificationEmailAddress;
            Data.RequestedInstanceType = message.RequestedInstanceType;

            AddNote(string.Format("Server start sequence started at {0} (UTC).", DateTime.UtcNow));

            Timeout(new CheckServerStatusCommand { InstanceId = Data.Instance.InstanceId });
        }

        public void Timeout(CheckServerStatusCommand state)
        {
            switch (Data.ServerStartUpStatus)
            {
                case ServerStartUpStatus.Initialized:
                    {
                        Ec2UtilityInstance instance = _instanceManager.GetInstance(Data.Instance.InstanceId);

                        if (instance.InstanceType.Equals(Data.RequestedInstanceType))
                        {
                            Data.ServerStartUpStatus = ServerStartUpStatus.ReSizing;
                        }
                        else
                        {
                            try
                            {
                                AddNote(string.Format("*** Changing server from size {0} to {1}. ***", instance.InstanceType, Data.RequestedInstanceType));
                                _instanceManager.ChangeInstanceType(Data.Instance.InstanceId, Data.RequestedInstanceType);
                            }
                            catch (InvalidInstanceTypeException e)
                            {
                                AbortServerStartUp(e.Message);
                                return;
                            }
                        }

                        break;
                    }
                case ServerStartUpStatus.ReSizing:
                    {
                        _instanceManager.StartUpInstance(Data.Instance.InstanceId);

                        Data.ServerStartUpStatus = ServerStartUpStatus.Starting;

                        break;
                    }
                case ServerStartUpStatus.Starting:
                    {
                        Ec2UtilityInstance instance = _instanceManager.GetInstance(Data.Instance.InstanceId);

                        if (instance.Status == Ec2UtilityInstanceStatus.Running)
                            Data.ServerStartUpStatus = ServerStartUpStatus.Started;

                        break;
                    }
                case ServerStartUpStatus.Started:
                    {
                        _instanceManager.AssignInstanceIp(Data.Instance.InstanceId);

                        Data.ServerStartUpStatus = ServerStartUpStatus.IpAssigned;

                        break;
                    }
                case ServerStartUpStatus.IpAssigned:
                    {
                        AddNote(string.Format("Server start sequence finished at {0} (UTC).", DateTime.UtcNow));

                        SentServerStartedMessage();

                        Data.ServerStartUpStatus = ServerStartUpStatus.Complete;

                        break;
                    }
                case ServerStartUpStatus.Complete:
                    {
                        MarkAsComplete();

                        break;
                    }
            }

            var reply = new ServerStatusMessage
            {
                InstanceId = Data.Instance.InstanceId,
                StartUpStatus = Data.ServerStartUpStatus
            };

            ReplyToOriginator(reply);
            RequestUtcTimeout(TimeSpan.FromSeconds(5), new CheckServerStatusCommand { InstanceId = Data.Instance.InstanceId });
        }

        private void AddNote(string note)
        {
            if (null == Data.Notes)
            {
                Data.Notes = note;
            }
            else
            {
                Data.Notes = Data.Notes + Environment.NewLine + note;   
            }
        }

        private void AbortServerStartUp(string reason)
        {
            string body = "Reason for abort:" + Environment.NewLine + reason;

            _instanceManager.SendStartUpEmail(Data.Instance.InstanceId, Data.NotificationEmailAddress, "Server StartUp Aborted", body);

            var reply = new ServerStatusMessage
            {
                InstanceId = Data.Instance.InstanceId,
                StartUpStatus = ServerStartUpStatus.Complete
            };

            ReplyToOriginator(reply);

            MarkAsComplete();
        }

        private void SentServerStartedMessage()
        {
            string subject = string.Format("Instance '{0}' Started", Data.Instance.InstanceName);

            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine(subject);
            bodyBuilder.AppendLine(string.Empty);
            bodyBuilder.AppendLine("Details:");
            bodyBuilder.AppendLine(string.Empty);
            bodyBuilder.AppendLine(string.Format("Instance Id: {0}", Data.Instance.InstanceId));
            bodyBuilder.AppendLine(string.Format("Instance IP: {0}", Data.Instance.DefaultIp));
            bodyBuilder.AppendLine(string.Format("Instance Type: {0}", Data.RequestedInstanceType));
            bodyBuilder.AppendLine(string.Empty);
            
            if (!string.IsNullOrWhiteSpace(Data.Notes))
            {
                bodyBuilder.AppendLine("Notes:");
                bodyBuilder.AppendLine(string.Empty);
                bodyBuilder.AppendLine(Data.Notes);
            }

            _instanceManager.SendStartUpEmail(Data.Instance.InstanceId, Data.NotificationEmailAddress, subject, bodyBuilder.ToString());

            MarkAsComplete();
        }
    }
}
