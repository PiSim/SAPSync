using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Infrastructure
{
    public enum JobStatus
    {
        Idle,
        OnQueue,
        Ready,
        Running,
        Aborted,
        Failed,
        Completed,
        Stopped
    }
}
