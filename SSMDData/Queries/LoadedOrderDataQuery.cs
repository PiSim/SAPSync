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
                .Include(wpld => wpld.Order)
                    .ThenInclude(ord => ord.OrderData)
                    .ThenInclude(od => od.Material)
                    .ThenInclude(mat => mat.ColorComponent);
        }

    }
}
