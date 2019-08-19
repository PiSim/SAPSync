using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class OrdersQuery : Query<Order, SSMDContext>
    {
        #region Methods

        public override IQueryable<Order> Execute(SSMDContext context)
        {
            IQueryable<Order> query = context.Orders;

            if (LazyLoadingDisabled)
                context.ChangeTracker.LazyLoadingEnabled = false;

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}