using DataAccessCore;
using DataAccessCore.Commands;
using DMTAgent.Infrastructure;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMTAgent.SyncElements.SyncOperations
{
    public class RecordWriter<T> : IRecordWriter<T> where T : class
    {
        #region Constructors

        public RecordWriter(IRecordEvaluator<T> recordEvaluator,
            IDataService<SSMDContext> dataService)
        {
            SSMDData = dataService;
            RecordEvaluator = recordEvaluator;
            Packages = new List<UpdatePackage<T>>();

            ChildrenTasks = new List<Task>();
        }

        #endregion Constructors

        #region Events

        public event EventHandler<SyncErrorEventArgs> ErrorRaised;

        #endregion Events

        #region Properties

        public ICollection<Task> ChildrenTasks { get; }
        public IRecordEvaluator<T> RecordEvaluator { get; }
        protected Task LoadTask { get; set; }
        protected ICollection<UpdatePackage<T>> Packages { get; }

        protected virtual IDataService<SSMDContext> SSMDData { get; }

        #endregion Properties

        #region Methods

        public void CloseWriter()
        {
            Clear();
        }

        public virtual async Task CommitAsync() => await StartChildTask(() => Commit());
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

        public virtual void OpenWriter()
        {
            RecordEvaluator.Initialize(SSMDData);
            EnsureInitialized();
        }

        public async void OpenWriterAsync()
        {
            LoadTask = StartChildTask(OpenWriter);
            await LoadTask;
        }

        public void WriteRecords(IEnumerable<T> records)
        {
            // Nothing to do if enumerable is empty
            if (records.Count() == 0)
                return;


            // Ensure initialization has been completed before attempting to write
            LoadTask.Wait();

            try
            {
                Packages.Add(RecordEvaluator.GetUpdatePackage(records));
            }
            catch (Exception e)
            {
                RaiseError(e: e,
                    errorMessage: "Errore nell'importazione dei record " + e.Message + "\t\tInnerException : " + e.InnerException?.Message,
                    errorSeverity: SyncErrorEventArgs.ErrorSeverity.Major);
            }
        }

        public async void WriteRecordsAsync(IEnumerable<T> records) => await StartChildTask(() => WriteRecords(records));

        protected virtual void Clear()
        {
            RecordEvaluator.Clear();
            Packages.Clear();
        }

        protected virtual void CommitPackage(UpdatePackage<T> updatePackage)
        {
            InsertNewRecords(updatePackage.RecordsToInsert);
            UpdateExistingRecords(updatePackage.RecordsToUpdate);
            DeleteRecords(updatePackage.RecordsToDelete);
        }

        protected virtual void DeleteRecords(IEnumerable<T> records)
        {
            SSMDData.Execute(new DeleteEntitiesCommand<SSMDContext>(records));
        }

        protected virtual void EnsureInitialized()
        {
            if (RecordEvaluator == null)
                throw new InvalidOperationException("Evaluator non inizializzato");
            RecordEvaluator.CheckInitialized();
        }

        protected virtual void InsertNewRecords(IEnumerable<T> records)
        {
            BatchInsertEntitiesCommand<SSMDContext> insertCommand = new BatchInsertEntitiesCommand<SSMDContext>(records);
            SSMDData.Execute(insertCommand);
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

        protected virtual Task StartChildTask(Action action)
        {
            Task newTask = new Task(action);
            ChildrenTasks.Add(newTask);
            newTask.Start();
            return newTask;
        }

        protected virtual void UpdateExistingRecords(IEnumerable<T> records)
        {
            BatchUpdateEntitiesCommand<SSMDContext> updateCommand = new BatchUpdateEntitiesCommand<SSMDContext>(records);
            SSMDData.Execute(updateCommand);
        }

        #endregion Methods
    }
}