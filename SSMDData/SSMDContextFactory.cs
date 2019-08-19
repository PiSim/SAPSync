using Microsoft.EntityFrameworkCore.Design;

namespace SSMD
{
    public class SSMDContextFactory : IDesignTimeDbContextFactory<SSMDContext>
    {
        #region Methods

        public SSMDContext CreateDbContext(string[] args)
        {
            return new SSMDContext();
        }

        #endregion Methods
    }
}