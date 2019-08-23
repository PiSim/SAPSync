using DataAccessCore.Commands;
using SSMD;
using SAPSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPSync.Infrastructure;

namespace SAPSync.SyncElements.SyncOperations
{
    public class SyncData<T> : SyncOperationBase  where T : class
    {
        public SyncData(IRecordReader<T> recordReader,
            IRecordWriter<T> recordWriter) : base()
        {
            RecordWriter = recordWriter;
            RecordReader = recordReader;
            RecordWriter.ErrorRaised += OnErrorRaised;
            RecordReader.ErrorRaised += OnErrorRaised;
            RecordReader.RecordPacketCompleted += OnReaderPacketComplete;
            RecordReader.ReadCompleted += OnReadCompleteAsync;
        }

        protected virtual void OnErrorRaised(object sender, SyncErrorEventArgs e)
        {

        }

        public override string Name => "SyncData";
        
        public IRecordWriter<T> RecordWriter { get; }

        public IRecordReader<T> RecordReader { get; }

        public override void Start(ISubJob newJob)
        {
            base.Start(newJob);
            RecordReader.OpenReader();
            RecordWriter.OpenWriterAsync();
            RecordReader.StartReadAsync();
        }

        protected virtual void OnReaderPacketComplete(object sender, RecordPacketCompletedEventArgs<T> e) => RecordWriter.WriteRecordsAsync(e.Packet);
        
        protected async virtual void OnReadCompleteAsync(object sender, EventArgs e)
        {
            RecordReader.CloseReader();
            await Task.Run(() => Task.WaitAll(RecordWriter.ChildrenTasks.ToArray()));
            CloseJob();
        }

        protected override void CloseJob()
        {
            RecordWriter.CloseWriter();
            base.CloseJob();
        }
    }
}
