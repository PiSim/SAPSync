using DataAccessCore;
using SSMD;

namespace DMTAgent.SyncElements.Validators
{
    public interface IRecordValidator<T>
    {
        #region Methods

        bool CheckIndexesInitialized();

        T GetInsertableRecord(T record);

        void InitializeIndexes(IDataService<SSMDContext> sSMDData);

        bool IsValid(T record);

        #endregion Methods
    }

    public class RecordValidator<T> : IRecordValidator<T> where T : class
    {
        #region Methods

        public bool CheckIndexesInitialized() => true;

        public T GetInsertableRecord(T record) => record;

        public void InitializeIndexes(IDataService<SSMDContext> sSMDData)
        {
        }

        public bool IsValid(T record) => true;

        #endregion Methods
    }
}