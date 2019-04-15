using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SSMD.Queries
{
    public class MaterialsQuery : Query<Material, SSMDContext>
    {
        #region Methods

        public override IQueryable<Material> Execute(SSMDContext context)
        {
            IQueryable<Material> query = context.Materials;

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}