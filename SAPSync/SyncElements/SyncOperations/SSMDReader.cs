using DataAccessCore;
using SAPSync.Infrastructure;
using SAPSync.SyncElements;
using SSMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SSMDReader<T> : IRecordReader<T> where T:class
    {
        public event EventHandler<SyncErrorEventArgs> ErrorRaised;

        protected virtual SSMDData GetSSMDData() => new SSMDData(new SSMDContextFactory());
        public SSMDReader(Func<Query<T, SSMDContext>> getQueryDelegate = null)
        {
            if (getQueryDelegate != null)
                GetQueryFunc = getQueryDelegate;
        }

        protected virtual Func<Query<T, SSMDContext>> GetQueryFunc { get; } = new Func<Query<T, SSMDContext>>(() => new Query<T, SSMDContext>());

        protected virtual Query<T, SSMDContext> GetQuery() => GetQueryFunc();

        protected virtual IQueryable<T> RunQuery() => GetSSMDData().RunQuery(GetQuery());

        public virtual IEnumerable<T> ReadRecords() => RunQuery().ToList();
    }

    public class SSMDReader<TQueried, TOut> : IRecordReader<TOut> where TQueried : class
    {
        public event EventHandler<SyncErrorEventArgs> ErrorRaised;

        protected virtual SSMDData GetSSMDData() => new SSMDData(new SSMDContextFactory());

        public SSMDReader(Func<IQueryable<TQueried>, IQueryable<TOut>> translatorDelegate,
            Func<Query<TQueried, SSMDContext>> getQueryDelegate = null)
        {
            TranslatorFunc = translatorDelegate;
            GetQueryFunc = getQueryDelegate;
        }
        
        protected virtual Func<IQueryable<TQueried>, IQueryable<TOut>> TranslatorFunc { get; }
        protected virtual Func<Query<TQueried, SSMDContext>> GetQueryFunc { get; } = new Func<Query<TQueried, SSMDContext>>(() => new Query<TQueried, SSMDContext>());

        protected virtual IQueryable<TOut> RunQuery() => TranslatorFunc(GetSSMDData().RunQuery(GetQueryFunc()));

        public virtual IEnumerable<TOut> ReadRecords() => RunQuery().ToList();
    }
    
}
