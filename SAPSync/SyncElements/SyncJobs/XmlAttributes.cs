using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SAPSync.SyncElements.SyncJobs
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

    public class Value : Attribute
    {

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
}
