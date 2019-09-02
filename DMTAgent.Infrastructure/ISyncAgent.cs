using System;

namespace DMTAgent.Infrastructure
{
    public enum AgentStatus
    {
        Running,
        Stopped,
        Idle
    }

    public interface ISyncAgent
    {
        #region Events

        event EventHandler StatusChanged;

        #endregion Events

        #region Properties

        AgentStatus Status { get; }

        #endregion Properties

        #region Methods

        void Start();

        void Stop();

        #endregion Methods
    }
}