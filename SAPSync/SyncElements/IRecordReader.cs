using SyncService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{
    public interface IRecordReader<T> : ISyncBase, IDisposable
    {
        IEnumerable<T> ReadRecords();     
    }
}
