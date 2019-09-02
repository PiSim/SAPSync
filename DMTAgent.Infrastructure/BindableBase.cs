using System.ComponentModel;

namespace DMTAgent.Infrastructure
{
    public class BindableBase : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Methods

        protected virtual void RaisePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));

        #endregion Methods
    }
}