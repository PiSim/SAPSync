using DataAccessCore;
using SAPSync.SyncElements;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SSMDReader<T> : SyncElementBase, IRecordReader<T> where T:class
    {
        public SSMDReader(Func<Query<T, SSMDContext>> getQueryDelegate = null)
        {
            if (getQueryDelegate != null)
                GetQueryFunc = getQueryDelegate;
        }

        protected virtual Func<Query<T, SSMDContext>> GetQueryFunc { get; } = new Func<Query<T, SSMDContext>>(() => new Query<T, SSMDContext>());

        public override string Name => "SSMDReader";

        protected virtual Query<T, SSMDContext> GetQuery() => GetQueryFunc();

        protected virtual IQueryable<T> RunQuery() => GetSSMDData().RunQuery(GetQuery());

        public IEnumerable<T> ReadRecords() => RunQuery().ToList();
    }
}
