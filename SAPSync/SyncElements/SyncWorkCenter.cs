using DataAccessCore;
using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync
{
    public class SyncWorkCenters : SyncElement<WorkCenter>
    {
        #region Constructors

        private Dictionary<int, WorkCenter> _workCenterDictionary;

        public SyncWorkCenters()
        {
            Name = "Centri Di Lavoro";
        }

        #endregion Constructors

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();
            _workCenterDictionary = _sSMDData.RunQuery(new Query<WorkCenter,SSMDContext>()).ToDictionary(insp => insp.ID);

        }

        protected override void EnsureInitialized()
        {
            base.EnsureInitialized();
            if (_workCenterDictionary == null)
                throw new InvalidOperationException("Impossibile recuperare il dizionario Centri");
        }

        protected override bool MustIgnoreRecord(WorkCenter record) => _workCenterDictionary.ContainsKey(record.ID);

        protected override IList<WorkCenter> ReadRecordTable() => new ReadWorkCenters().Invoke(_rfcDestination);

        #endregion Methods
    }
}