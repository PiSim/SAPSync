using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class LoadedOrderScrapCauseConfirmationsQuery : Query<OrderConfirmation, SSMDContext>
    {
        #region Methods

        public override IQueryable<OrderConfirmation> Execute(SSMDContext context)
        {
            return base.Execute(context)
                .Include(wpld => wpld.Order)
                    .ThenInclude(ord => ord.OrderData)
                        .ThenInclude(od => od.Material)
                        .ThenInclude(mat => mat.ColorComponent)
                .Include(wpld => wpld.Order)
                    .ThenInclude(ord => ord.OrderData)
                        .ThenInclude(od => od.Material)
                            .ThenInclude(mat => mat.MaterialFamily)
                                .ThenInclude(mfa => mfa.L1);
        }

        #endregion Methods
    }
}