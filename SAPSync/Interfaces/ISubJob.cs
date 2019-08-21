using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.Interfaces
{
    public interface ISubJob
    {
        IJob ParentJob { get; }

        ICollection<IUnitOfWork> UnitsOfWork {get;}
    }
}
