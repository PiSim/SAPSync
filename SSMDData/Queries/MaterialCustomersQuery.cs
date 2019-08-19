using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class MaterialCustomersQuery : Query<MaterialCustomer, SSMDContext>
    {
        #region Methods

        public override IQueryable<MaterialCustomer> Execute(SSMDContext context)
        {
            IQueryable<MaterialCustomer> query = base.Execute(context);

            if (EagerLoadingEnabled)
                query = query.Include(mac => mac.Material);

            return query;
        }

        #endregion Methods
    }
}