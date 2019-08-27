using SAP.Middleware.Connector;
using SSMD;

namespace DMTAgent.SAP
{
    public class ReadWorkCenters : ReadTableBase<WorkCenter>
    {
        #region Constructors

        public ReadWorkCenters() : base()
        {
            _tableName = "CRHD";
            _fields = new string[]
            {
                "OBJID",
                "ARBPL"
            };
        }

        #endregion Constructors

        #region Methods

        internal override WorkCenter ConvertRow(IRfcStructure row)
        {
            WorkCenter output = new WorkCenter();

            string[] data = row.GetString("WA").Split(_separator);

            if (!int.TryParse(data[0], out int workCenterID))
                return null;

            output = new WorkCenter()
            {
                ID = workCenterID,
                ShortName = data[1]
            };

            return output;
        }

        #endregion Methods
    }
}