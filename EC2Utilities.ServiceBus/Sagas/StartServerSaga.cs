using System;
using EC2Utilities.Common.Contract.Messages;
using NServiceBus.Saga;

namespace EC2Utilities.ServiceBus.Sagas
{
    public class StartServerSaga : Saga<StartServerSagaData>, IAmStartedByMessages<StartServerCommand>
    {
        public void Handle(StartServerCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
