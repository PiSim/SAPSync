using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMTAgent.Infrastructure
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
    }
}
