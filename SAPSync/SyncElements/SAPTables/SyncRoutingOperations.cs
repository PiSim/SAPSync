using SAPSync.Functions;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class RoutingOperationEvaluator : RecordEvaluator<RoutingOperation, Tuple<long, int>>
    {
        #region Methods

        protected override void ConfigureRecordValidator()
        {
            RecordValidator = new RoutingOperationValidator();
        }

        protected override Tuple<long, int> GetIndexKey(RoutingOperation record) => new Tuple<long, int>(record.RoutingNumber, record.RoutingCounter);

        #endregion Methods
    }

    public class SyncRoutingOperations : SyncSAPTable<RoutingOperation>
    {
        #region Constructors

        public SyncRoutingOperations(SyncElementConfiguration configuration) : base(configuration)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "Operazioni Ordine";

        #endregion Properties

        #region Methods

        protected override void ExecuteExport(IEnumerable<RoutingOperation> records)
        {
        }

        protected override IRecordEvaluator<RoutingOperation> GetRecordEvaluator() => new RoutingOperationEvaluator();

        protected virtual IList<RoutingOperation> MergeReadTables(IList<RoutingOperation> mainTable, IList<RoutingOperation> secondaryTable)
        {
            IDictionary<Tuple<long, int>, RoutingOperation> index = secondaryTable.ToDictionary(secondary => new Tuple<long, int>(secondary.RoutingNumber, secondary.RoutingCounter), main => main);

            foreach (RoutingOperation mainRecord in mainTable)
            {
                Tuple<long, int> key = new Tuple<long, int>(mainRecord.RoutingNumber, mainRecord.RoutingCounter);
                if (index.ContainsKey(key))
                {
                    RoutingOperation secondaryRecord = index[key];
                    mainRecord.BaseQuantity = secondaryRecord.BaseQuantity;
                }
            }

            return mainTable;
        }

        protected override IList<RoutingOperation> ReadRecordTable()
        {
            return MergeReadTables((new ReadRoutingOperations()).Invoke(_rfcDestination), (new ReadRoutingOperationValues()).Invoke(_rfcDestination));
        }

        #endregion Methods

    }
}