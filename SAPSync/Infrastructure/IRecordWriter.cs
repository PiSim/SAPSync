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
        void OpenWriter();
        void OpenWriterAsync();
        void WriteRecords(IEnumerable<T> records);
        void WriteRecordsAsync(IEnumerable<T> records);
        event EventHandler<SyncErrorEventArgs> ErrorRaised;
        ICollection<Task> ChildrenTasks { get; }
        void Commit();
        void CloseWriter();
    }
}
