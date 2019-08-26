using System;

namespace SAPSync.SyncElements.SyncOperations
{
    public class Column : System.Attribute
    {
        #region Constructors

        public Column(int column)
        {
            ColumnIndex = column;
        }

        #endregion Constructors

        #region Properties

        public int ColumnIndex { get; set; }

        #endregion Properties
    }

    public class Exported : Attribute
    {
    }

    public class FontColor : Attribute
    {
    }

    public class Imported : Attribute
    {
    }

    public class Value : Attribute
    {
    }
}