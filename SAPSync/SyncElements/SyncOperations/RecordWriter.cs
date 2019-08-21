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
    public class RecordWriter<T> : SyncElementBase, IRecordWriter<T> where T: class
    {    

        public RecordWriter(IRecordEvaluator<T> recordEvaluator)
        {
            RecordEvaluator = recordEvaluator;
        }

        protected virtual IEnumerable<T> Records { get; set; }
            
        public IRecordEvaluator<T> RecordEvaluator { get; }

        public override string Name => "RecordWriter";
        
        public void WriteRecords(IEnumerable<T> records)
        {
            try
            {
                Records = records;
                Run();
            }
            catch (Exception e)
            {
                RaiseSyncError(e: e,
                    errorMessage: "Errore nell'importazione dei record " + e.Message + "\t\tInnerException : " + e.InnerException.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
            }
        }

        protected override void Execute()
        {
            base.Execute();
            var updatePackage = RecordEvaluator.GetUpdatePackage(Records);
            UpdateDatabase(updatePackage);
        }

        protected override void Clear()
        {
            Records = null;
            RecordEvaluator.Clear();
        }

        protected virtual void InsertNewRecords(IEnumerable<T> records)
        {
            RaiseProgressChanged(0);
            BatchInsertEntitiesCommand<SSMDContext> insertCommand = new BatchInsertEntitiesCommand<SSMDContext>(records);
            insertCommand.ProgressChanged += OnSubscribedProgressChanged;
            GetSSMDData().Execute(insertCommand);
        }

        protected virtual void UpdateExistingRecords(IEnumerable<T> records)
        {
            RaiseProgressChanged(0);
            BatchUpdateEntitiesCommand<SSMDContext> updateCommand = new BatchUpdateEntitiesCommand<SSMDContext>(records);
            updateCommand.ProgressChanged += OnSubscribedProgressChanged;
            GetSSMDData().Execute(updateCommand);
        }

        protected virtual void UpdateDatabase(UpdatePackage<T> updatePackage)
        {
            InsertNewRecords(updatePackage.RecordsToInsert);
            UpdateExistingRecords(updatePackage.RecordsToUpdate);
            DeleteRecords(updatePackage.RecordsToDelete);
        }

        protected override void Initialize()
        {
            base.Initialize();
            RecordEvaluator.Initialize(GetSSMDData());
        }

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();
            if (RecordEvaluator == null)
                throw new InvalidOperationException("Evaluator non inizializzato");
        }

        protected virtual void DeleteRecords(IEnumerable<T> records)
        {
            GetSSMDData().Execute(new DeleteEntitiesCommand<SSMDContext>(records));
        }
    }
}
