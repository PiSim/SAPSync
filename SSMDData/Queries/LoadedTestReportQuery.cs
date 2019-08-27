using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class LoadedTestReportQuery : Query<TestReport, SSMDContext>
    {
        #region Methods

        public override IQueryable<TestReport> Execute(SSMDContext context) => base.Execute(context)
            .Include(trp => trp.Order)
                .ThenInclude(ord => ord.OrderData)
                    .ThenInclude(odd => odd.Material)
                        .ThenInclude(mat => mat.ColorComponent)
            .Include(trp => trp.Order)
                .ThenInclude(ord => ord.OrderData)
                .ThenInclude(odd => odd.Material)
                    .ThenInclude(mat => mat.MaterialFamily)
                        .ThenInclude(mtf => mtf.L1)
            .Include(trp => trp.Order)
                .ThenInclude(ord => ord.OrderData)
                    .ThenInclude(odd => odd.Material)
                        .ThenInclude(mat => mat.MaterialCustomer)
                            .ThenInclude(mac => mac.Customer)
            .Include(trp => trp.Order)
                .ThenInclude(odd => odd.OrderData)
                .ThenInclude(ord => ord.Material)
                    .ThenInclude(mat => mat.Project)
                        .ThenInclude(prj => prj.WBSRelations)
                            .ThenInclude(wbr => wbr.Up)
                                .ThenInclude(wbu => wbu.WBSRelations)
                                    .ThenInclude(wur => wur.Up)
            .Include(trp => trp.Order)
                .ThenInclude(ord => ord.InspectionLots)
                    .ThenInclude(inl => inl.InspectionPoints)
                        .ThenInclude(inp => inp.InspectionSpecification)
                            .ThenInclude(ins => ins.InspectionCharacteristic)
            .OrderByDescending(rep => rep.Number);

        #endregion Methods
    }
}