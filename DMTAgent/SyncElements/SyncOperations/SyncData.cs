using DMTAgent.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMTAgent.SyncElements.SyncOperations
{
    public class SyncData<T> : SyncOperationBase where T : class
    {
        #region Constructors

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

        #endregion Constructors

        #region Properties

        public override string Name => "SyncData";

        public IRecordReader<T> RecordReader { get; }

        public IRecordWriter<T> RecordWriter { get; }

        #endregion Properties

        #region Methods

        public override void Start(ISubJob newJob)
        {
            base.Start(newJob);
            RecordReader.OpenReader();
            RecordWriter.OpenWriterAsync();
            RecordReader.StartReadAsync();
        }

        protected override void CloseJob()
        {
            RecordWriter.CloseWriter();
            base.CloseJob();
        }

        protected virtual void OnErrorRaised(object sender, SyncErrorEventArgs e)
        {
        }

        protected async virtual void OnReadCompleteAsync(object sender, EventArgs e)
        {
            RecordReader.CloseReader();
            await Task.Run(() => Task.WaitAll(RecordWriter.ChildrenTasks.ToArray()));
            await RecordWriter.CommitAsync();
            CloseJob();
        }

        protected virtual void OnReaderPacketComplete(object sender, RecordPacketCompletedEventArgs<T> e) => RecordWriter.WriteRecordsAsync(e.Packet);

        #endregion Methods
    }
}