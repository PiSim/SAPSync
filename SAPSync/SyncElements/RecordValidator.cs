using SSMD;

namespace SAPSync.SyncElements
{
    public interface IRecordValidator<T>
    {
        #region Methods

        bool CheckIndexesInitialized();

        T GetInsertableRecord(T record);

        void InitializeIndexes(SSMDData sSMDData);

        bool IsValid(T record);

        #endregion Methods
    }

    public class RecordValidator<T> : IRecordValidator<T> where T : class
    {
        #region Methods

        public bool CheckIndexesInitialized() => true;

        public T GetInsertableRecord(T record) => record;

        public void InitializeIndexes(SSMDData sSMDData)
        {
            return;
        }

        public bool IsValid(T record) => true;

        #endregion Methods
    }
}