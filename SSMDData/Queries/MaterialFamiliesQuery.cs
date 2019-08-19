using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class MaterialFamiliesQuery : Query<MaterialFamily, SSMDContext>
    {
        #region Methods

        public override IQueryable<MaterialFamily> Execute(SSMDContext context)
        {
            IQueryable<MaterialFamily> query = context.MaterialFamilies;

            if (EagerLoadingEnabled)
                query = query.Include(mfl => mfl.L1)
                    .Include(mfl => mfl.L2)
                    .Include(mfl => mfl.L3);

            return query;
        }

        #endregion Methods
    }
}