using SAPSync;
using System;
using System.ComponentModel;

namespace SAPSync
{
    public interface ISyncBase
    {

        string Name { get; }

        event ProgressChangedEventHandler ProgressChanged;

        event EventHandler<TaskEventArgs> ExternalTaskCompleted;

        event EventHandler<TaskEventArgs> ExternalTaskStarting;

        event EventHandler StatusChanged;

        event EventHandler<SyncErrorEventArgs> SyncErrorRaised;
        event EventHandler ElementStarting;
        event EventHandler ElementCompleted;
    }
}