using EC2Utilities.Common.Contract;
using EC2Utilities.Common.Contract.Messages;
using EC2Utilities.Common.Manager;
using NServiceBus;
using NServiceBus.Saga;
using StructureMap;

namespace EC2Utilities.ServiceBus.Sagas
{
    public class StartServerSaga : Saga<StartServerSagaData>, IAmStartedByMessages<StartServerCommand>, IHandleMessages<CheckServerStatusCommand>
    {
        private readonly IInstanceManager _instanceManager;

        public StartServerSaga()
        {
            _instanceManager = ObjectFactory.GetInstance<IInstanceManager>();
        }

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<CheckServerStatusCommand>(x => x.InstanceId, y => y.InstanceId);
        }
        
        public void Handle(StartServerCommand message)
        {
            Data.InstanceId = message.InstanceId;
            Data.NotificationEmailAddress = message.NotificationEmailAddress;
        }

        public void Handle(CheckServerStatusCommand message)
        {
            switch (Data.ServerStartUpStatus)
            {
                case ServerStartUpStatus.Initialized:
                    {
                        _instanceManager.StartUpInstance(Data.InstanceId);

                        Data.ServerStartUpStatus = ServerStartUpStatus.Starting;

                        break;
                    }
                case ServerStartUpStatus.Starting:
                    {
                        Ec2UtilityInstance instance = _instanceManager.GetInstance(Data.InstanceId);

                        if (instance.Status == Ec2UtilityInstanceStatus.Running)
                            Data.ServerStartUpStatus = ServerStartUpStatus.Started;

                        break;
                    }
                case ServerStartUpStatus.Started:
                    {
                        _instanceManager.AssignInstanceIp(Data.InstanceId);

                        Data.ServerStartUpStatus = ServerStartUpStatus.IpAssigned;

                        break;
                    }
                case ServerStartUpStatus.IpAssigned:
                    {
                        _instanceManager.SendServerAvailableNotification(Data.InstanceId, Data.NotificationEmailAddress);
                        
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
                                InstanceId = Data.InstanceId,
                                StartUpStatus = Data.ServerStartUpStatus
                            };

            ReplyToOriginator(reply);
        }
    }
}
