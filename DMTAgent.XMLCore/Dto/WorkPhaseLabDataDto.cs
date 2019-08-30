using SSMD;
using System.Linq;

namespace DMTAgent.XMLCore
{
    public class WorkPhaseLabDataDto : DtoBase<WorkPhaseLabData>
    {
        #region Properties

        [Column(16), Imported, Value, Exported]
        public string Actions { get; set; }

        [Column(15), Imported, Value, Exported]
        public string Analysis { get; set; }

        [Column(5), Value, Exported]
        public string Aspect { get; set; }

        [Column(2), Value, Exported]
        public string MaterialCode { get; set; }

        [Column(12), Imported, Value, Exported]
        public string NotesC { get; set; }

        [Column(13), Imported, Value, Exported]
        public string NotesG { get; set; }

        [Column(14), Imported, Value, Exported]
        public string NotesP { get; set; }

        [Column(11), Imported, Value, Exported]
        public string NotesS { get; set; }

        [Column(4), Value, Exported]
        public double? OrderAmount { get; set; }

        [Column(1), Imported, Value, Exported]
        public int OrderNumber { get; set; }

        [Column(3), Value, Exported]
        public string OrderType { get; set; }

        [Column(9), Value, Exported]
        public string PCA { get; set; }

        [Column(10), Value, Exported]
        public string ProjectDescription { get; set; }

        [Column(7), Value, Exported]
        public string Structure { get; set; }

        [Column(8), Imported, Value, Exported]
        public string TrialScope { get; set; }

        #endregion Properties

        #region Methods

        public override void SetValues(WorkPhaseLabData entity)
        {
            base.SetValues(entity);
            MaterialCode = entity.Order?.OrderData?.FirstOrDefault()?.Material?.Code;
            OrderType = entity.Order?.OrderType;
            Structure = entity.Order?.OrderData?.FirstOrDefault()?.Material?.MaterialFamily?.L1?.Code;
            Aspect = entity.Order?.OrderData?.FirstOrDefault()?.Material?.ColorComponent?.Description.Replace("SKIN", "");
            PCA = entity.Order?.OrderData?.FirstOrDefault()?.Material?.Project?.WBSUpRelations?.FirstOrDefault()?.Up?.Code;
            ProjectDescription = entity.Order?.OrderData?.FirstOrDefault()?.Material?.Project?.WBSUpRelations?.FirstOrDefault()?.Up?.Description2;
            OrderAmount = entity.Order?.OrderData?.FirstOrDefault()?.PlannedQuantity;
        }

        #endregion Methods
    }
}