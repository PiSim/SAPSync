using DataAccessCore.Commands;
using SAPSync.Infrastructure;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAPSync.SyncElements.SyncOperations
{
    public class RecordWriter<T> : IRecordWriter<T> where T : class
    {
        public RecordWriter(IRecordEvaluator<T> recordEvaluator)
        {
            RecordEvaluator = recordEvaluator;
            Packages = new List<UpdatePackage<T>>();

            ChildrenTasks = new List<Task>();
        }

        public IRecordEvaluator<T> RecordEvaluator { get; }

        public event EventHandler<SyncErrorEventArgs> ErrorRaised;

        protected ICollection<UpdatePackage<T>> Packages { get; }

        public async void WriteRecordsAsync(IEnumerable<T> records) => await StartChildTask(() => WriteRecords(records));

        public void WriteRecords(IEnumerable<T> records)
        {
            try
            {
                Packages.Add(RecordEvaluator.GetUpdatePackage(records));
            }
            catch (Exception e)
            {
                RaiseError(e: e,
                    errorMessage: "Errore nell'importazione dei record " + e.Message + "\t\tInnerException : " + e.InnerException.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
            }
        }

        protected virtual Task StartChildTask(Action action)
        {
            Task newTask = new Task(action);
            ChildrenTasks.Add(newTask);
            newTask.Start();
            return newTask;
        }
        public ICollection<Task> ChildrenTasks { get; }

        protected SSMDData GetSSMDData() => new SSMDData(new SSMDContextFactory());

        protected virtual void Clear()
        {
            RecordEvaluator.Clear();
            Packages.Clear();
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

        protected virtual void CommitPackage(UpdatePackage<T> updatePackage)
        {
            InsertNewRecords(updatePackage.RecordsToInsert);
            UpdateExistingRecords(updatePackage.RecordsToUpdate);
            DeleteRecords(updatePackage.RecordsToDelete);
        }

        public virtual void OpenWriter()
        {
            RecordEvaluator.Initialize(GetSSMDData());
            EnsureInitialized();
        }

        protected virtual void EnsureInitialized()
        {
            if (RecordEvaluator == null)
                throw new InvalidOperationException("Evaluator non inizializzato");
            RecordEvaluator.CheckInitialized();
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


        public void Commit()
        {
            foreach (UpdatePackage<T> package in Packages.Where(pkg => pkg.IsCommitted == false))
            {
                package.IsCommitted = true;
                try
                {
                    CommitPackage(package);
                }
                catch (Exception e)
                {
                    RaiseError(e);
                    package.IsCommitted = false;
                }
            }
        }

        public void CloseWriter()
        {
            Clear();
        }

        public async void OpenWriterAsync() => await StartChildTask(OpenWriter);
    }
}
