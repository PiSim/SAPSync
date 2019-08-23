using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SAP.Middleware.Connector;
using SAPSync.Infrastructure;
using SAPSync.SyncElements;
using SSMD;

namespace SAPSync.RFCFunctions
{
    public class InspLotGetList : BAPITableFunctionBase, IRecordReader<InspectionLot>
    {
        #region Constructors

        public InspLotGetList() : base()
        {
            _functionName = "BAPI_INSPLOT_GETLIST";
            _tableName = "insplot_LIST";
        }

        public event EventHandler<SyncErrorEventArgs> ErrorRaised;
        public event EventHandler<RecordPacketCompletedEventArgs<InspectionLot>> RecordPacketCompleted;
        public event EventHandler ReadCompleted;

        protected void ReadRecords()
        { 
            IEnumerable<InspectionLot> records = ConvertInspectionLotTable(Invoke(new SAPReader().GetRfcDestination()));
            RaisePacketCompleted(records);
            RaiseReadCompleted();
        }

        protected virtual void RaisePacketCompleted(IEnumerable<InspectionLot> records)
        {
            RecordPacketCompleted?.Invoke(this, new RecordPacketCompletedEventArgs<InspectionLot>(records));
        }

        protected virtual void RaiseReadCompleted() => ReadCompleted?.Invoke(this, new EventArgs());


        private List<InspectionLot> ConvertInspectionLotTable(IRfcTable materialTable)
        {
            List<InspectionLot> output = new List<InspectionLot>();

            foreach (IRfcStructure row in materialTable)
            {
                string lotNumberString = row.GetString("insplot");

                if (!long.TryParse(lotNumberString, out long currentLotNumber))
                    continue;

                InspectionLot newInspectionLot = new InspectionLot
                {
                    Number = currentLotNumber
                };

                string orderNumberString = row.GetString("order_no");

                if (!int.TryParse(orderNumberString, out int orderNumber))
                    continue;

                newInspectionLot.OrderNumber = orderNumber;

                output.Add(newInspectionLot);
            }

            return output;
        }

        #endregion Constructors

        #region Methods

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
            _rfcFunction.SetValue("max_rows", 99999999);
        }

        public async void StartReadAsync() => await Task.Run(() => ReadRecords());

        public void OpenReader()
        {
            throw new NotImplementedException();
        }

        public void CloseReader()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}