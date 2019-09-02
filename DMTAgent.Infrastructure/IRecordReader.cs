using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMTAgent.Infrastructure
{
    public interface IRecordReader<T>
    {
        #region Events

        event EventHandler<SyncErrorEventArgs> ErrorRaised;

        event EventHandler ReadCompleted;

        event EventHandler<RecordPacketCompletedEventArgs<T>> RecordPacketCompleted;

        #endregion Events

        #region Properties

        ICollection<Task> ChildrenTasks { get; }

        #endregion Properties

        #region Methods

        void CloseReader();

        void OpenReader();

        void StartReadAsync();

        #endregion Methods
    }

    public class RecordPacketCompletedEventArgs<T> : EventArgs
    {
        #region Constructors

        public RecordPacketCompletedEventArgs(IEnumerable<T> records, bool isFinal = false)
        {
            Packet = records;
            IsFinal = isFinal;
        }

        #endregion Constructors

        #region Properties

        public bool IsFinal { get; }
        public IEnumerable<T> Packet { get; }

        #endregion Properties
    }
}