using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessCore;
using Microsoft.EntityFrameworkCore;

namespace SSMD.Queries
{
    public class OrderComponentsQuery : Query<OrderComponent , SSMDContext>
    {
        public override IQueryable<OrderComponent> Execute(SSMDContext context)
        {
            IQueryable<OrderComponent> query = base.Execute(context);

            if (EagerLoadingEnabled)
                query = query.Include(rec => rec.Component);

            return query;
        }
    }
}
