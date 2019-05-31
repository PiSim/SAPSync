using DataAccessCore;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public enum SyncAction
    {
        Update,
        Insert,
        Delete,
        Ignore
    }

    public interface IRecordEvaluator<T>
    {
        #region Methods

        bool CheckIndexInitialized();

        IEnumerable<SyncItem<T>> EvaluateRecords(IEnumerable<T> records);

        void InitializeIndex(SSMDData sSMDData);

        T SetPrimaryKeyForExistingRecord(T record);
        
        #endregion Methods
    }

    public abstract class RecordEvaluator<T, TKey> : IRecordEvaluator<T> where T : class
    {

        public RecordEvaluator()
        {
            _trackedRecordIndex = new Dictionary<TKey, T>();
        }

        #region Fields

        protected IDictionary<TKey, T> _recordIndex;
        protected IDictionary<TKey, T> _trackedRecordIndex;

        #endregion Fields

        #region Properties

        public virtual bool CheckRemovedRecords { get; set; } = true;

        public virtual bool IgnoreExistingRecords { get; set; } = false;

        public IDictionary<TKey, T> RecordIndex => _recordIndex;
        public IDictionary<TKey, T> TrackedRecordIndex => _trackedRecordIndex;

        #endregion Properties

        #region Methods

        public bool CheckIndexInitialized() => _recordIndex != null;


        public IEnumerable<SyncItem<T>> EvaluateRecords(IEnumerable<T> records)
        {
            List<SyncItem<T>> retrievedItems = records.Select(rec => new SyncItem<T>(rec)).ToList();

            foreach (SyncItem<T> record in retrievedItems)
                record.Action = GetRecordDesignation(record.Item);

            if (CheckRemovedRecords)
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

        public virtual void InitializeIndex(SSMDData sSMDData)
        {
            _recordIndex = sSMDData.RunQuery(new Query<T, SSMDContext>()).ToDictionary(rec => GetIndexKey(rec), rec => rec);
        }

        public virtual T SetPrimaryKeyForExistingRecord(T record) => record;

        protected virtual void AddToTrackedRecordIndex(T record) => _trackedRecordIndex.Add(GetIndexKey(record), record);

        protected abstract TKey GetIndexKey(T record);

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

                else if (!IgnoreExistingRecords)
                    return SyncAction.Update;
            }

            return SyncAction.Ignore;
        }

        public virtual T GetIndexedEntry(TKey key)
        {
            if (RecordIndex.ContainsKey(key))
                return RecordIndex[key];
            else if (TrackedRecordIndex.ContainsKey(key))
                return TrackedRecordIndex[key];
            else return null;
        }

        protected virtual bool IsTracked(T record) => TrackedRecordIndex.ContainsKey(GetIndexKey(record));

        protected virtual bool IsExistingRecord(T record) => RecordIndex.ContainsKey(GetIndexKey(record));

        protected virtual bool MustIgnoreRecord(T record) => false;

        #endregion Methods
    }

    public class SyncItem<T>
    {
        #region Fields

        private readonly T _item;

        #endregion Fields

        #region Constructors

        public SyncItem(T item)
        {
            _item = item;
        }

        #endregion Constructors

        #region Properties

        public SyncAction Action { get; set; } = SyncAction.Ignore;
        public T Item => _item;

        #endregion Properties
    }
}