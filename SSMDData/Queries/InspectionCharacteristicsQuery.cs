using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class InspectionCharacteristicsQuery : Query<InspectionCharacteristic, SSMDContext>
    {
        #region Methods

        public override IQueryable<InspectionCharacteristic> Execute(SSMDContext context)
        {
            IQueryable<InspectionCharacteristic> query = context.InspectionCharacteristics;

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}