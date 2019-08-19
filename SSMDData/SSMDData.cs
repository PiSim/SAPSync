using DataAccessCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SSMD
{
    public class SSMDData : DataServiceBase<SSMDContext>
    {
        #region Fields

        private readonly IDesignTimeDbContextFactory<SSMDContext> _dbContextFactory;

        #endregion Fields

        #region Constructors

        public SSMDData(IDesignTimeDbContextFactory<SSMDContext> dbContextFactory) : base(dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        #endregion Constructors
    }
}