using Microsoft.EntityFrameworkCore.Design;
using SSMD;
using System;

namespace DMTAgent
{
    // I don't like this implementation but right now I can't find another way to retrieve a new SSMDContext on demand
    // without reinstantiating everything
    public class SSMDContextFactory : IDesignTimeDbContextFactory<SSMDContext>
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion Fields

        #region Constructors

        public SSMDContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public SSMDContext CreateDbContext(string[] args) => (SSMDContext)_serviceProvider.GetService(typeof(SSMDContext));

        #endregion Methods
    }
}