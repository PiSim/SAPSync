using SAP.Middleware.Connector;
using SAPSync.Infrastructure;
using SSMD;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPSync.RFCFunctions
{
    public class InspLotGetList : BAPITableFunctionBase, IRecordReader<InspectionLot>
    {
        #region Constructors

        public InspLotGetList() : base()
        {
            _functionName = "BAPI_INSPLOT_GETLIST";
            _tableName = "insplot_LIST";
            ChildrenTasks = new List<Task>();
        }

        #endregion Constructors

        #region Events

        public event EventHandler<SyncErrorEventArgs> ErrorRaised;

        public event EventHandler ReadCompleted;

        public event EventHandler<RecordPacketCompletedEventArgs<InspectionLot>> RecordPacketCompleted;

        #endregion Events

        #region Properties

        public ICollection<Task> ChildrenTasks { get; }

        #endregion Properties

        #region Methods

        public void CloseReader()
        {
        }

        public void OpenReader()
        {
        }

        public async void StartReadAsync() => await Task.Run(() => ReadRecords());

        protected override void InitializeParameters()
        {
            base.InitializeParameters();
            _rfcFunction.SetValue("max_rows", 99999999);
        }

        protected virtual void RaisePacketCompleted(IEnumerable<InspectionLot> records)
        {
            RecordPacketCompleted?.Invoke(this, new RecordPacketCompletedEventArgs<InspectionLot>(records));
        }

        protected virtual void RaiseReadCompleted() => ReadCompleted?.Invoke(this, new EventArgs());

        protected void ReadRecords()
        {
            IEnumerable<InspectionLot> records = ConvertInspectionLotTable(Invoke(new SAPReader().GetRfcDestination()));
            RaisePacketCompleted(records);
            RaiseReadCompleted();
        }

        protected virtual Task StartChildTask(Action action)
        {
            Task newTask = new Task(action);
            ChildrenTasks.Add(newTask);
            newTask.Start();
            return newTask;
        }

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

        #endregion Methods
    }
}