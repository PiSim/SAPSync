using SSMD;

namespace SAPSync.Functions
{
    public class ReadWBSRelations : ReadTableBase<WBSRelation>
    {
        #region Constructors

        public ReadWBSRelations() : base()
        {
            _tableName = "PRHI";

            _fields = new string[]
            {
                "POSNR",
                "PSPHI",
                "UP",
                "DOWN",
                "LEFT",
                "RIGHT"
            };
        }

        #endregion Constructors

        #region Methods

        internal override WBSRelation ConvertDataArray(string[] data)
        {
            if (!int.TryParse(data[0], out int relationID)
                || !int.TryParse(data[1], out int projectID)
                || !int.TryParse(data[2], out int upID)
                || !int.TryParse(data[3], out int downID)
                || !int.TryParse(data[4], out int leftID)
                || !int.TryParse(data[5], out int rightID))
                return null;

            WBSRelation output = new WBSRelation()
            {
                ID = relationID,
                ProjectID = projectID,
                UpID = upID,
                DownID = downID,
                LeftID = leftID,
                RightID = rightID
            };

            return output;
        }

        #endregion Methods
    }
}