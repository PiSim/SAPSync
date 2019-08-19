using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class OrderComponentsQuery : Query<OrderComponent, SSMDContext>
    {
        #region Methods

        public override IQueryable<OrderComponent> Execute(SSMDContext context)
        {
            IQueryable<OrderComponent> query = base.Execute(context);

            if (EagerLoadingEnabled)
                query = query.Include(rec => rec.Component);

            return query;
        }

        #endregion Methods
    }
}