﻿using System.Collections.Generic;

namespace EC2Utilities.Common.ResourceAccess
{
    public interface IConfigResourceAccess
    {
        List<string> GetInstances();

        Ec2Key GetEc2Key();

        string GetServiceName();

        int GetBackupRetentionDays();
    }
}