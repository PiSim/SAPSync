using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class InspectionPointsQuery : Query<InspectionPoint, SSMDContext>
    {
        #region Methods

        public override IQueryable<InspectionPoint> Execute(SSMDContext context)
        {
            IQueryable<InspectionPoint> query = context.InspectionPoints;

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}