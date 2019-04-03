using DataAccessCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SSMD
{
    public class SSMDData : IDataService<SSMDContext>
    {
        #region Fields

        private IDesignTimeDbContextFactory<SSMDContext> _dbContextFactory;

        #endregion Fields

        #region Constructors

        public SSMDData(IDesignTimeDbContextFactory<SSMDContext> dbContextFactory) : base()
        {
            _dbContextFactory = dbContextFactory;
        }

        #endregion Constructors

        #region Methods

        public void Execute(ICommand<SSMDContext> commandObject)
        {
            commandObject.Execute(_dbContextFactory.CreateDbContext(new string[] { }));
        }

        public T RunQuery<T>(IScalar<T, SSMDContext> queryObject)
        {
            return queryObject.Execute(_dbContextFactory.CreateDbContext(new string[] { }));
        }

        public System.Linq.IQueryable<T> RunQuery<T>(IQuery<T, SSMDContext> queryObject)
        {
            return queryObject.Execute(_dbContextFactory.CreateDbContext(new string[] { }));
        }

        #endregion Methods
    }
}