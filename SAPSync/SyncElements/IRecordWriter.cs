using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements
{
    public interface IRecordWriter<T> : SyncService.ISyncBase where T : class
    {
        void WriteRecords(IEnumerable<T> records);        
    }
}
