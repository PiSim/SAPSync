using System;
using System.Threading.Tasks;

namespace SAPSync
{
    public class TaskEventArgs : EventArgs
    {
        #region Constructors

        public TaskEventArgs(Task t)
        {
            ReadingTask = t;
        }

        #endregion Constructors

        #region Properties

        public Task ReadingTask { get; }

        #endregion Properties
    }
}