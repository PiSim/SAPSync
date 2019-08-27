namespace SAPSync.Infrastructure
{
    public enum JobStatus
    {
        Idle,
        OnQueue,
        Ready,
        Running,
        Aborted,
        Failed,
        Completed,
        Stopped
    }
}