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
            IEnumerable<T> records;
            try
            {
                Task<IEnumerable<T>> getResultsTask = new Task<IEnumerable<T>>(() => RecordReader.ReadRecords());
                getResultsTask.Start();
                await getResultsTask;
                records = getResultsTask.Result ?? throw new Exception("Lettura Record Fallita");
            }
            catch (Exception e)
            {
                RaiseSyncError(e: e,
                    errorMessage: "Errore di lettura: " + e.Message + "\t\tInnerException :" + e.InnerException.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
                throw new Exception("Lettura Record Fallita: " + e.Message, e);
            }

            try
            {
                Task writeRecordsTask = new Task(() => RecordWriter.WriteRecords(records));
                writeRecordsTask.Start();
                writeRecordsTask.Wait();
            }
            catch (Exception e)
            {
                RaiseSyncError(e: e,
                    errorMessage: "Errore di scrittura: " + e.Message + "\t\tInnerException :" + e.InnerException.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
                throw new Exception("Scrittura Record Fallita: " + e.Message, e);
            }

        }

    }
}
