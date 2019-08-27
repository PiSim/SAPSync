using System;

namespace DMTAgent.XML
{
    public class ModifyRangeToken
    {
        #region Properties

        public Tuple<int, int> EndCell { get; set; }
        public string RangeName { get; set; }
        public string SheetIndex { get; set; }
        public Tuple<int, int> StartCell { get; set; }
        public string Value { get; set; }

        #endregion Properties
    }
}