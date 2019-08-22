using SAPSync;
using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface IRecordReader<T>
    {
        IEnumerable<T> ReadRecords();
        event EventHandler<SyncErrorEventArgs> ErrorRaised;
    }
}
