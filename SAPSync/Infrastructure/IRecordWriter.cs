using SAPSync.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPSync
{
    public interface IRecordWriter<T> where T : class
    {
        #region Events

        event EventHandler<SyncErrorEventArgs> ErrorRaised;

        #endregion Events

        #region Properties

        ICollection<Task> ChildrenTasks { get; }

        #endregion Properties

        #region Methods

        void CloseWriter();

        void Commit();

        void OpenWriter();

        void OpenWriterAsync();

        void WriteRecords(IEnumerable<T> records);

        void WriteRecordsAsync(IEnumerable<T> records);

        #endregion Methods
    }
}