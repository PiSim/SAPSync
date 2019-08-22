using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface IRecordWriter<T>  where T : class
    {
        void WriteRecords(IEnumerable<T> records);
        event EventHandler<SyncErrorEventArgs> ErrorRaised;
    }
}
