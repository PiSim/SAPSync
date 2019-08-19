using SAPSync.SyncElements;
using SyncService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SyncTask : SyncElementBase, ISyncTask
    {
        public event EventHandler SyncTaskCompleted;
        public event EventHandler SyncTaskStarting;
        public event EventHandler SyncTaskStarted;
        public List<string> SyncLog { get; set; }
        public List<Task> TaskList { get; }

        public SyncTask(IEnumerable<ISyncElement> syncElements)
        {
            if (syncElements.Any(se => se.CurrentTask != null))
                throw new InvalidOperationException("SyncElement is already part of an active Task");

            PendingSyncElements = new HashSet<ISyncElement>(syncElements);
            ActiveSyncElements = new HashSet<ISyncElement>();
            CompletedSyncElements = new List<ISyncElement>();
            
            ActiveReadTasks = new HashSet<Task>();
            TaskList = new List<Task>();

            foreach (ISyncElement syncElement in PendingSyncElements)
            {
                syncElement.SetCurrentTask(this);
                SubscribeToElement(syncElement);
            }
        }
        
        public void Start()
        {
            RaiseSyncTaskStarting();
            StartReadyPendingElements();
            RaiseSyncTaskStarted();
        }

        protected virtual void StartReadyPendingElements()
        {
            foreach (ISyncElement element in PendingSyncElements.ToList())
                if (!element.HasPendingRequirements)
                    StartElement(element);
        }

        protected virtual void StartElement(ISyncElement element)
        {
            Task elementTask = new Task(() => element.StartSync());
            TaskList.Add(elementTask);

            ActiveSyncElements.Add(element);
            if (PendingSyncElements.Contains(element))
                PendingSyncElements.Remove(element);

            elementTask.Start();

        }

        protected virtual void OnReadTaskComplete(object sender, TaskEventArgs e)
        {
            if (ActiveReadTasks.Contains(e.ReadingTask))
                ActiveReadTasks.Remove(e.ReadingTask);
        }

        protected virtual void OnReadTaskStarting(object sender, TaskEventArgs e)
        {
            ActiveReadTasks.Add(e.ReadingTask);
        }

        public ICollection<ISyncElement> PendingSyncElements { get; }
        public ICollection<Task> ActiveReadTasks { get; }

        protected virtual void OnSyncElementStarting(object sender, EventArgs e)
        {
            ActiveSyncElements.Add(sender as ISyncElement);
        }
        
        protected override void OnSubscribedElementCompleted(object sender, EventArgs e)
        {
            base.OnSubscribedElementCompleted(sender, e);
            if (!(sender is ISyncElement))
                return;

            ISyncElement element = sender as ISyncElement;
            
            CompletedSyncElements.Add(element);
            if (ActiveSyncElements.Contains(element))
                ActiveSyncElements.Remove(element);

            StartReadyPendingElements();

            if (CheckAllElementsComplete())
                OnCompleting();
        }
        protected override void OnCompleting()
        {
            foreach (ISyncBase element in CompletedSyncElements)
                UnsubscribeFromElement(element);
            RaiseSyncTaskCompleted();
            base.OnCompleting();
        }

        protected virtual void RaiseSyncTaskStarted() => SyncTaskStarted?.Invoke(this, new EventArgs());
        
        protected virtual void RaiseSyncTaskStarting() => SyncTaskStarting?.Invoke(this, new EventArgs());
        
        protected virtual void RaiseSyncTaskCompleted() => SyncTaskCompleted?.Invoke(this, new EventArgs());
        
        public ICollection<ISyncElement> ActiveSyncElements { get; }
        public ICollection<ISyncElement> CompletedSyncElements { get; }

        public override string Name => "SyncTask";

        protected virtual bool CheckAllElementsComplete() => ActiveSyncElements.Count == 0 && PendingSyncElements.Count == 0;
    }
}
