using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class LoadedWorkPhaseLabDataQuery : Query<WorkPhaseLabData, SSMDContext>
    {
        #region Methods

        public override IQueryable<WorkPhaseLabData> Execute(SSMDContext context)
        {
            return base.Execute(context)
                .Include(wpld => wpld.Order)
                    .ThenInclude(ord => ord.OrderData)
                    .ThenInclude(od => od.Material)
                    .ThenInclude(mat => mat.ColorComponent);
        }

        #endregion Methods
    }
}