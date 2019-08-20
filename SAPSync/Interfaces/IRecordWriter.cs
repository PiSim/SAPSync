using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface IRecordWriter<T> : ISyncBase where T : class
    {
        void WriteRecords(IEnumerable<T> records);        
    }
}
