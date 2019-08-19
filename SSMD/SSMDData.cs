using System;
using System.Collections.Generic;
using System.Text;
using DataAccessCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SSMD
{
    public class SSMDData : IDataService<SSMDContext>
    {
        IDesignTimeDbContextFactory<SSMDContext> _dbContextFactory;

        public SSMDData(IDesignTimeDbContextFactory<SSMDContext> dbContextFactory) : base()
        {
            _dbContextFactory = dbContextFactory;
        }

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
    }
}
