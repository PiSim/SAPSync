using SAPSync;
using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{

    public class RecordPacketCompletedEventArgs<T> : EventArgs
    {
        public RecordPacketCompletedEventArgs(IEnumerable<T> records, bool isFinal = false)
        {
            Packet = records;
            IsFinal = isFinal;
        }

        public IEnumerable<T> Packet { get; }
        public bool IsFinal { get; }
    }

    public interface IRecordReader<T>
    {
        void OpenReader();
        void StartReadAsync();
        void CloseReader();
        ICollection<Task> ChildrenTasks { get; }
        event EventHandler<SyncErrorEventArgs> ErrorRaised;
        event EventHandler<RecordPacketCompletedEventArgs<T>> RecordPacketCompleted;
        event EventHandler ReadCompleted;
    }
}
