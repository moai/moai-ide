using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai
{
    public interface ISyncable
    {
        event EventHandler SyncDataChanged;

        ISyncData GetSyncData();
    }

    public interface ISyncData
    {
    }
}
