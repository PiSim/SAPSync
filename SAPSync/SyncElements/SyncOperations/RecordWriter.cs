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
    public class RecordWriter<T> :  IRecordWriter<T> where T: class
    {    

        public RecordWriter(IRecordEvaluator<T> recordEvaluator)
        {
            RecordEvaluator = recordEvaluator;
        }

        public IRecordEvaluator<T> RecordEvaluator { get; }

        public event EventHandler<SyncErrorEventArgs> ErrorRaised;

        public void WriteRecords(IEnumerable<T> records)
        {
            try
            {
                var updatePackage = RecordEvaluator.GetUpdatePackage(records);
                UpdateDatabase(updatePackage);
            }
            catch (Exception e)
            {
                RaiseError(e: e,
                    errorMessage: "Errore nell'importazione dei record " + e.Message + "\t\tInnerException : " + e.InnerException.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
            }
        }

        protected SSMDData GetSSMDData() => new SSMDData(new SSMDContextFactory());
        
        protected virtual void Clear()
        {
            RecordEvaluator.Clear();
        }

        protected virtual void InsertNewRecords(IEnumerable<T> records)
        {
            BatchInsertEntitiesCommand<SSMDContext> insertCommand = new BatchInsertEntitiesCommand<SSMDContext>(records);
            GetSSMDData().Execute(insertCommand);
        }

        protected virtual void UpdateExistingRecords(IEnumerable<T> records)
        {
            BatchUpdateEntitiesCommand<SSMDContext> updateCommand = new BatchUpdateEntitiesCommand<SSMDContext>(records);
            GetSSMDData().Execute(updateCommand);
        }

        protected virtual void UpdateDatabase(UpdatePackage<T> updatePackage)
        {
            InsertNewRecords(updatePackage.RecordsToInsert);
            UpdateExistingRecords(updatePackage.RecordsToUpdate);
            DeleteRecords(updatePackage.RecordsToDelete);
        }

        protected virtual void Initialize()
        {
            RecordEvaluator.Initialize(GetSSMDData());
        }

        protected virtual void EnsureInitialized()
        {
            if (RecordEvaluator == null)
                throw new InvalidOperationException("Evaluator non inizializzato");
        }

        protected virtual void DeleteRecords(IEnumerable<T> records)
        {
            GetSSMDData().Execute(new DeleteEntitiesCommand<SSMDContext>(records));
        }
        protected virtual void RaiseError(
           Exception e = null,
           string errorMessage = null,
           SyncErrorEventArgs.ErrorSeverity errorSeverity = SyncErrorEventArgs.ErrorSeverity.Minor)
        {
            SyncErrorEventArgs args = new SyncErrorEventArgs()
            {
                Exception = e,
                Severity = errorSeverity,
                ErrorMessage = errorMessage,
                TimeStamp = DateTime.Now,
                TypeOfElement = GetType()
            };

            ErrorRaised?.Invoke(this, args);
        }
    }
}
