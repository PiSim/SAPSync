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
            IRecordWriter<T> recordWriter)
        {
            RecordWriter = recordWriter;
            RecordReader = recordReader;
            RecordWriter.ErrorRaised += OnErrorRaised;
            RecordReader.ErrorRaised += OnErrorRaised;
        }

        protected virtual void OnErrorRaised(object sender, SyncErrorEventArgs e)
        {

        }

        public override string Name => "SyncData";
        
        public IRecordWriter<T> RecordWriter { get; }

        public IRecordReader<T> RecordReader { get; }

        public async override void Start()
        {
            base.Start();
            await Task.Run(() => RecordReader.StartReadAsync());
        }

        protected async virtual void OnReaderPacketCompleteAsync(object sender, RecordPacketCompletedEventArgs<T> e)
        {
            await Task.Run(() => RecordWriter.WriteRecords(e.Packet));
            if (e.IsFinal)
            {
                RecordWriter.Clear();
                RaiseOperationCompleted();
            }
        }        
    }
}
