using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class InspectionLotsQuery : Query<InspectionLot, SSMDContext>
    {
        #region Methods

        public override IQueryable<InspectionLot> Execute(SSMDContext context)
        {
            IQueryable<InspectionLot> query = context.InspectionLots;

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}