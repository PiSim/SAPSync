using DataAccessCore;
using SAPSync.SyncElements.Validators;
using SSMD;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public interface IRecordEvaluator<T>
    {
        #region Properties

        RecordEvaluatorConfiguration Configuration { get; }

        #endregion Properties

        #region Methods

        bool CheckInitialized();

        void Clear();

        UpdatePackage<T> GetUpdatePackage(IEnumerable<T> records);

        void Initialize(SSMDData sSMDData);

        #endregion Methods
    }

    public abstract class RecordEvaluator<T, TKey> : IRecordEvaluator<T> where T : class
    {
        #region Fields

        protected IDictionary<TKey, T> _recordIndex;

        protected IDictionary<TKey, T> _trackedRecordIndex;

        #endregion Fields

        #region Constructors

        public RecordEvaluator(RecordEvaluatorConfiguration configuration = null)
        {
            Configuration = configuration ?? new RecordEvaluatorConfiguration();
        }

        #endregion Constructors

        #region Enums

        protected enum SyncAction
        {
            Update,
            Insert,
            Delete,
            Ignore
        }

        #endregion Enums

        #region Properties

        public RecordEvaluatorConfiguration Configuration { get; }

        public IDictionary<TKey, T> RecordIndex => _recordIndex;

        public IDictionary<TKey, T> TrackedRecordIndex => _trackedRecordIndex;

        protected IRecordValidator<T> RecordValidator { get; set; }

        #endregion Properties

        #region Methods

        public bool CheckInitialized() => _recordIndex != null;

        public void Clear()
        {
            RecordValidator = null;
            _recordIndex = null;
            _trackedRecordIndex = null;
        }

        public virtual T GetIndexedEntry(TKey key)
        {
            if (RecordIndex.ContainsKey(key))
                return RecordIndex[key];
            else if (TrackedRecordIndex.ContainsKey(key))
                return TrackedRecordIndex[key];
            else return null;
        }

        public virtual UpdatePackage<T> GetUpdatePackage(IEnumerable<T> records)
        {
            IEnumerable<SyncItem<T>> evaluatedRecords = EvaluateRecords(records);

            var validRecords = evaluatedRecords.Where(
                rec => rec.Action != SyncAction.Ignore
                    && RecordValidator.IsValid(rec.Item))
                    .ToList();

            List<T> deleteRecords = new List<T>();
            List<T> insertRecords = new List<T>();
            List<T> updateRecords = new List<T>();

            foreach (SyncItem<T> record in validRecords)
            {
                T indexedRecord = GetInsertableRecord(record);
                if (record.Action == SyncAction.Delete)
                    deleteRecords.Add(indexedRecord);
                else if (record.Action == SyncAction.Insert)
                    insertRecords.Add(indexedRecord);
                else if (record.Action == SyncAction.Update)
                    updateRecords.Add(indexedRecord);
            }

            UpdatePackage<T> output = new UpdatePackage<T>(
                deleteRecords,
                insertRecords,
                updateRecords);

            return output;
        }

        public virtual void Initialize(SSMDData sSMDData)
        {
            _trackedRecordIndex = new Dictionary<TKey, T>();
            _recordIndex = sSMDData.RunQuery(GetIndexEntriesQuery()).ToDictionary(rec => GetIndexKey(rec), rec => rec);
            RecordValidator = GetRecordValidator();
            InitializeRecordValidator(sSMDData);
        }

        protected virtual void AddToTrackedRecordIndex(T record) => _trackedRecordIndex.Add(GetIndexKey(record), record);

        protected virtual IEnumerable<SyncItem<T>> EvaluateRecords(IEnumerable<T> records)
        {
            List<SyncItem<T>> retrievedItems = records.Select(rec => new SyncItem<T>(rec)).ToList();

            foreach (SyncItem<T> record in retrievedItems)
                record.Action = GetRecordDesignation(record.Item);

            if (Configuration.CheckRemovedRecords)
            {
                HashSet<TKey> existingKeysIndex = new HashSet<TKey>(retrievedItems.Select(rec => GetIndexKey(rec.Item)).Distinct());
                foreach (T record in _recordIndex.Values)
                    if (!existingKeysIndex.Contains(GetIndexKey(record)))
                    {
                        SyncItem<T> itemToRemove = new SyncItem<T>(record) { Action = SyncAction.Delete };
                        retrievedItems.Add(itemToRemove);
                    }
            }

            return retrievedItems;
        }

        protected virtual Query<T, SSMDContext> GetIndexEntriesQuery() => new Query<T, SSMDContext>();

        protected abstract TKey GetIndexKey(T record);

        protected virtual T GetInsertableRecord(SyncItem<T> syncItem)
        {
            T record = (syncItem.Action == SyncAction.Update || syncItem.Action == SyncAction.Delete) ? SetPrimaryKeyForExistingRecord(syncItem.Item) : syncItem.Item;
            return RecordValidator.GetInsertableRecord(record);
        }

        protected virtual SyncAction GetRecordDesignation(T record)
        {
            if (MustIgnoreRecord(record))
                return SyncAction.Ignore;
            else if (!IsTracked(record))
            {
                AddToTrackedRecordIndex(record);
                if (!IsExistingRecord(record))
                {
                    return SyncAction.Insert;
                }
                else if (!Configuration.IgnoreExistingRecords)
                    return SyncAction.Update;
            }

            return SyncAction.Ignore;
        }

        protected virtual IRecordValidator<T> GetRecordValidator() => new RecordValidator<T>();

        protected virtual void InitializeRecordValidator(SSMDData sSMDData)
        {
            RecordValidator.InitializeIndexes(sSMDData);
        }

        protected virtual bool IsExistingRecord(T record) => RecordIndex.ContainsKey(GetIndexKey(record));

        protected virtual bool IsTracked(T record) => TrackedRecordIndex.ContainsKey(GetIndexKey(record));

        protected virtual bool MustIgnoreRecord(T record) => false;

        protected virtual T SetPrimaryKeyForExistingRecord(T record) => record;

        #endregion Methods

        #region Classes

        protected class SyncItem<TI> where TI : T
        {
            #region Fields

            private readonly TI _item;

            #endregion Fields

            #region Constructors

            public SyncItem(TI item)
            {
                _item = item;
            }

            #endregion Constructors

            #region Properties

            public SyncAction Action { get; set; } = SyncAction.Ignore;
            public TI Item => _item;

            #endregion Properties
        }

        #endregion Classes
    }

    public class RecordEvaluatorConfiguration
    {
        #region Properties

        public bool CheckRemovedRecords { get; set; } = true;

        public bool IgnoreExistingRecords { get; set; } = false;

        #endregion Properties
    }

    public class UpdatePackage<T>
    {
        #region Fields

        private readonly IEnumerable<T> _recordsToUpdate,
            _recordsToInsert,
            _recordsToDelete;

        #endregion Fields

        #region Constructors

        public UpdatePackage(IEnumerable<T> recordsToDelete,
            IEnumerable<T> recordsToInsert,
            IEnumerable<T> recordsToUpdate)
        {
            _recordsToDelete = recordsToDelete.ToList();
            _recordsToInsert = recordsToInsert.ToList();
            _recordsToUpdate = recordsToUpdate.ToList();
        }

        #endregion Constructors

        #region Properties

        public bool IsCommitted { get; set; } = false;
        public IEnumerable<T> RecordsToDelete => _recordsToDelete;
        public IEnumerable<T> RecordsToInsert => _recordsToInsert;
        public IEnumerable<T> RecordsToUpdate => _recordsToUpdate;

        #endregion Properties
    }
}