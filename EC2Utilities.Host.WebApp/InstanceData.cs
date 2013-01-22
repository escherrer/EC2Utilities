using System;
using System.Collections.Generic;
using EC2Utilities.Common.Contract.Messages;

namespace EC2Utilities.Host.WebApp
{
    public static class InstanceData
    {
        private static readonly Dictionary<string, ServerStartUpStatus> InstanceStatuses;
        private static readonly Object This = new Object();

        static InstanceData()
        {
            InstanceStatuses = new Dictionary<string, ServerStartUpStatus>();
        }

        public static void SetStatus(string instanceId, ServerStartUpStatus serverStartUpStatus)
        {
            if (InstanceStatuses.ContainsKey(instanceId))
            {
                InstanceStatuses.Remove(instanceId);
            }

            InstanceStatuses.Add(instanceId, serverStartUpStatus);
        }

        public static ServerStartUpStatus GetServerStartUpStatus(string instanceId)
        {
            lock (This)
            {
                if (InstanceStatuses.ContainsKey(instanceId))
                {
                    return InstanceStatuses[instanceId];
                }
                
                return ServerStartUpStatus.Unknown;
            }
        }
    }
}