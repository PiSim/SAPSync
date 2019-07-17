using SSMD;

namespace SAPSync.Functions
{
    public class ReadProjects : ReadTableBase<Project>
    {
        public override string Name => "ReadProjects";
        #region Constructors

        public ReadProjects() : base()
        {
            _tableName = "PRPS";

            _fields = new string[]
            {
                "PSPNR",
                "STUFE",
                "POSID",
                "USR00",
                "POST1"
            };
        }

        #endregion Constructors

        #region Methods

        internal override Project ConvertDataArray(string[] data)
        {
            if (!int.TryParse(data[0], out int prjID)
                || !int.TryParse(data[1], out int wbsLevel))
                return null;

            string projectCode = data[2].Trim();
            string description = data[3];
            string description2 = data[4];

            Project output = new Project()
            {
                ID = prjID,
                Code = projectCode,
                Description = description,
                Description2 = description2,
                WBSLevel = wbsLevel
            };

            return output;
        }

        #endregion Methods
    }
}