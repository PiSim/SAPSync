using SSMD;
using SAPSync;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPSync.Infrastructure;

namespace SAPSync.SyncElements
{

    public abstract class SyncElementBase : IDisposable
    {
        public event EventHandler ElementStarting;
        public event EventHandler ElementCompleted;

        public abstract string Name { get; }

        protected virtual void Initialize()
        {

        }
        protected virtual void EnsureInitialized()
        {

        }

        public void Dispose()
        {
            Clear();
        }

        protected virtual void Clear()
        {
        }

                
        
        protected virtual void RaiseSyncError(
            Exception e = null,
            string errorMessage = null,
            SyncErrorEventArgs.ErrorSeverity errorSeverity = SyncErrorEventArgs.ErrorSeverity.Minor)
        {
            SyncErrorEventArgs args = new SyncErrorEventArgs()
            {
                Exception = e,
                NameOfElement = Name,
                Severity = errorSeverity,
                ErrorMessage = errorMessage,
                TimeStamp = DateTime.Now,
                TypeOfElement = GetType()
            };

        }
    }
}
