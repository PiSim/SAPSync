using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMTAgent.Infrastructure
{
    public interface IViewModel<T> where T : IView
    {
    }
}
