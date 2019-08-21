using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMD.Queries
{
    public class LoadedOrderDataQuery : Query<OrderData, SSMDContext>
    {
        public override IQueryable<OrderData> Execute(SSMDContext context)
        {
            return base.Execute(context)
                .Include(odd => odd.Order)
                    .ThenInclude(ord => ord.TestReports)
                .Include(odd => odd.Order)
                    .ThenInclude(ord => ord.WorkPhaseLabData)
                .Include(odd => odd.Material)
                    .ThenInclude(mat => mat.ColorComponent)
                .Include(odd => odd.Material)
                    .ThenInclude(mat => mat.MaterialFamily)
                    .ThenInclude(mfa => mfa.L1)
                .Include(odd => odd.Order)
                    .ThenInclude(ord => ord.OrderConfirmations)
                        .ThenInclude(oco => oco.WorkCenter)
                .Include(odd => odd.Material)
                    .ThenInclude(mat => mat.Project)
                        .ThenInclude(prj => prj.WBSRelations)
                            .ThenInclude(wbr => wbr.Up)
                                .ThenInclude(upj => upj.WBSRelations)
                                    .ThenInclude(uwb => uwb.Up)
                .Include(odd => odd.Order)
                    .ThenInclude(ord => ord.InspectionLots)
                        .ThenInclude(inl => inl.InspectionPoints)
                            .ThenInclude(inp => inp.InspectionSpecification)
                                .ThenInclude(ins => ins.InspectionCharacteristic)
                .Include(odd => odd.Order)
                    .ThenInclude(ord => ord.OrderComponents)
                        .ThenInclude(oco => oco.Component)
                .Where(odd => odd.Order.OrderType[0] == 'Z')
                .OrderByDescending(odd => odd.OrderNumber);
        }

    }
}
