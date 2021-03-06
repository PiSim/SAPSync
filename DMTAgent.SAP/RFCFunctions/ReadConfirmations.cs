﻿using SAP.Middleware.Connector;
using SSMD;
using System;
using System.Globalization;

namespace DMTAgent.SAP
{
    public class ReadConfirmations : ReadTableBase<OrderConfirmation>
    {
        #region Constructors

        public ReadConfirmations() : base()
        {
            _separator = new char[] { '|' };
            _tableName = "AFRU";
            _fields = new string[]
            {
                "RUECK",
                "RMZHL",
                "ERSDA",
                "ISBD",
                "ISBZ",
                "IEBD",
                "IEBZ",
                "STZHL",
                "LMNGA",
                "XMNGA",
                "GRUND",
                "GMEIN",
                "AUFNR",
                "ARBID",
                "ZWIP_IN",
                "ZWIP_OUT",
                "STOKZ",
                "ZZREWORK",
                "ZZRW_BUONA",
                "ZZRW_SCARTO"
            };
        }

        #endregion Constructors

        #region Methods

        internal override OrderConfirmation ConvertRow(IRfcStructure row)
        {
            string[] data = row.GetString("WA").Split(_separator);

            int confirmationNumber = int.Parse(data[0].TrimStart(new char[] { '0' }));
            int confirmationCounter = int.Parse(data[1].TrimStart(new char[] { '0' }));

            DateTime acquisitionDate = DateStringToDate(data[2]);
            DateTime startTime = DateStringToDate(data[3], data[4]);
            DateTime endTime = DateStringToDate(data[5], data[6]);

            bool deletionFlag = (data[7] != "00000000" || data[16] == "X");
            double yield = double.Parse(data[8], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            double scrap = double.Parse(data[9], System.Globalization.NumberStyles.Float, new NumberFormatInfo() { NumberDecimalSeparator = "." });
            string scrapCause = data[10].Trim();
            string um = data[11];

            if (!int.TryParse(data[12], out int orderNumber))
                return null;

            int wcID = int.Parse(data[13]);
            string wipIn = data[14];
            string wipOut = data[15];

            bool isRework = data[16] == "x";

            OrderConfirmation converted = new OrderConfirmation()
            {
                ConfirmationNumber = confirmationNumber,
                ConfirmationCounter = confirmationCounter,
                DeletionFlag = deletionFlag,
                Yield = yield,
                Scrap = scrap,
                ScrapCause = scrapCause,
                UM = um,
                OrderNumber = orderNumber,
                WorkCenterID = wcID,
                WIPIn = wipIn,
                WIPOut = wipOut,
                EntryDate = acquisitionDate,
                StartTime = startTime,
                EndTime = endTime,
                IsRework = isRework
            };

            return converted;
        }

        protected override void ConfigureBatchingOptions()
        {
            BatchingOptions = new ReadTableBatchingOptions()
            {
                BatchSize = 50000,
                StringFormat = "000000000000",
                Field = "AUFNR",
                MinValue = 1000000,
                MaxValue = 1999999
            };
            base.ConfigureBatchingOptions();
        }

        #endregion Methods
    }
}