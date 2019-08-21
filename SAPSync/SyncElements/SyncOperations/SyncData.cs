using DataAccessCore.Commands;
using SSMD;
using SAPSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.SyncOperations
{
    public class SyncData<T> : SyncOperation where T : class
    {
        public SyncData(IRecordReader<T> recordReader,
            IRecordWriter<T> recordWriter)
        {
            RecordWriter = recordWriter;
            RecordReader = recordReader;

            SubscribeToElement(RecordReader);
            SubscribeToElement(RecordWriter);
        }

        public override string Name => "SyncData";
        
        public IRecordWriter<T> RecordWriter { get; }

        public IRecordReader<T> RecordReader { get; }

        protected override void Execute()
        {
            base.Execute();
            IEnumerable<T> records;
            try
            {
                Task<IEnumerable<T>> getResultsTask = new Task<IEnumerable<T>>(() => RecordReader.ReadRecords());
                RaiseExternalTaskStarting(getResultsTask);
                getResultsTask.Start();
                getResultsTask.Wait();
                RaiseExternalTaskCompleted(getResultsTask);
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
                RaiseExternalTaskStarting(writeRecordsTask);
                writeRecordsTask.Start();
                writeRecordsTask.Wait();
                RaiseExternalTaskCompleted(writeRecordsTask);
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
