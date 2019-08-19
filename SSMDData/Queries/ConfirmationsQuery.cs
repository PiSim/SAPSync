using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class ConfirmationsQuery : Query<OrderConfirmation, SSMDContext>
    {
        #region Methods

        public override IQueryable<OrderConfirmation> Execute(SSMDContext context)
        {
            IQueryable<OrderConfirmation> query = context.OrderConfirmations;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (LazyLoadingDisabled)
                context.ChangeTracker.LazyLoadingEnabled = false;

            return query;
        }

        #endregion Methods
    }
}