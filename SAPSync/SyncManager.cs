using SAPSync.SyncElements;
using SAPSync.SyncElements.ExcelWorkbooks;
using SAPSync.SyncElements.SAPTables;
using SSMD;
using SyncService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SyncManager : ISyncManager
    {
        #region Fields

        private List<ISyncElement> _syncElements;
        private List<Task> _taskList;

        #endregion Fields

        #region Constructors

        public SyncManager()
        {
            _taskList = new List<Task>();
            ActiveSyncs = new HashSet<ISyncElement>();
        }

        #endregion Constructors

        #region Events

        public event EventHandler NewSyncTaskStarted;

        public event EventHandler NewSyncTaskStarting;

        public event EventHandler SyncTaskCompleted;

        #endregion Events

        #region Properties

        public ICollection<ISyncElement> ActiveSyncs { get; }
        public bool HasRunningTasks => _taskList.Where(tsk => tsk.Status == TaskStatus.Running).Count() != 0;

        public ICollection<ISyncElement> SyncElements
        {
            get
            {
                if (_syncElements == null)
                {
                    ConfigureSyncElements();
                    InitializeSyncElements();
                }

                return _syncElements;
            }
        }

        public bool UpdateRunning { get; set; }

        #endregion Properties

        #region Methods

        public Task GetAwaiterForOpenReadTasks()
        {
            return Task.WhenAll(_taskList.Where(tsk => tsk?.Status == TaskStatus.Running).ToList());
        }

        public SSMDData GetSSMDData() => new SSMDData(new SSMDContextFactory());

        public DateTime? GetTimeForNextUpdate() => SyncElements.Min(sel => sel.NextScheduledUpdate);

        public void StartSync()
        {
            if (SyncElements.Count != 0 && !UpdateRunning)
            {
                UpdateRunning = true;
                RaiseNewSyncTaskStarting();
                RaiseNewSyncTaskStarted();
            }
        }

        protected virtual bool CheckAllElementsComplete() => ActiveSyncs.Count == 0;

        protected virtual void ConfigureSyncElements()
        {
            ISyncElement WorkCentersElement = new SyncWorkCenters(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                });

            ISyncElement MaterialFamilyLevelsElement = new SyncMaterialFamilylevels(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                });

            ISyncElement MaterialFamiliesElement = new SyncMaterialFamilies(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                        MaterialFamilyLevelsElement
                });

            ISyncElement ProjectsElement = new SyncProjects(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                });

            ISyncElement WBSRelationsElement = new SyncWBSRelations(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                        ProjectsElement
                });

            ISyncElement MaterialsElement = new SyncMaterials(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                        MaterialFamiliesElement,
                        ProjectsElement
                });

            ISyncElement OrdersElement = new SyncOrders(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                        MaterialsElement
                });

            ISyncElement OrderDataElement = new SyncOrderData(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement
                });

            ISyncElement RoutingOperationsElement = new SyncRoutingOperations(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    WorkCentersElement,
                    OrderDataElement
                });

            ISyncElement ComponentsElement = new SyncComponents(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                });

            ISyncElement ConfirmationsElements = new SyncConfirmations(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    WorkCentersElement
                });

            ISyncElement OrderComponentsElement = new SyncOrderComponents(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    ComponentsElement
                });

            ISyncElement InspectionCharacteristicsElement = new SyncInspectionCharacteristics(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                });

            ISyncElement InspectionLotsElement = new SyncInspectionLots(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement
                });

            ISyncElement CustomersElement = new SyncCustomers(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = false,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                });

            ISyncElement MaterialCustomerElement = new SyncMaterialCustomers(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement,
                    CustomersElement
                });

            ISyncElement InspectionSpecificationsElement = new SyncInspectionSpecifications(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    InspectionCharacteristicsElement
                });

            ISyncElement InspectionPointsElement = new SyncInspectionPoints(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    InspectionLotsElement,
                    InspectionSpecificationsElement,
                });

            ISyncElement TrialMasterListElement = new SyncTrialMasterReportTEST(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = false,
                    IgnoreExistingRecords = false,
                    PerformExport = true,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    OrderDataElement
                });

            ISyncElement TrialLabDataElement = new SyncTrialLabData(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = true,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    OrderDataElement,
                    TrialMasterListElement
                });

            ISyncElement TestReportsElement = new SyncTestReports(
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = true,
                    PerformImport = true
                })
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    OrderDataElement,
                    TrialMasterListElement
                });

            _syncElements = new List<ISyncElement>
                {
                    WorkCentersElement,
                    CustomersElement,
                    MaterialFamilyLevelsElement,
                    MaterialFamiliesElement,
                    MaterialCustomerElement,
                    ProjectsElement,
                    WBSRelationsElement,
                    MaterialsElement,
                    OrdersElement,
                    ComponentsElement,
                    RoutingOperationsElement,
                    OrderDataElement,
                    OrderComponentsElement,
                    ConfirmationsElements,
                    InspectionCharacteristicsElement,
                    InspectionLotsElement,
                    InspectionSpecificationsElement,
                    InspectionPointsElement,
                    TrialMasterListElement,
                    TrialLabDataElement,
                    TestReportsElement
                };
        }

        protected virtual void CreateLogEntry(string message)
        {
            string logPath = Properties.Settings.Default.LogFilePath;
            string logString = DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + message;
            List<string> logLines = new List<string>
            {
                logString
            };

            File.AppendAllLines(logPath, logLines);
        }

        protected virtual void InitializeSyncElements()
        {
            try
            {
                foreach (ISyncElement syncElement in _syncElements)
                {
                    NewSyncTaskStarted += syncElement.OnSyncTaskStarted;
                    NewSyncTaskStarting += syncElement.OnSyncTaskStarting;
                    syncElement.SyncErrorRaised += OnSyncErrorRaised;
                    syncElement.SyncFailed += OnSyncFailureRaised;
                    syncElement.SyncCompleted += OnSyncElementCompleted;
                    syncElement.ReadTaskCompleted += OnReadTaskComplete;
                    syncElement.ReadTaskStarting += OnReadTaskStarting;
                    syncElement.SyncElementStarting += OnSyncElementStarting;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Inizializzazione elementi di sincronizzazione fallita: " + e.Message);
            }
        }

        protected virtual void OnReadTaskComplete(object sender, TaskEventArgs e)
        {
        }

        protected virtual void OnReadTaskStarting(object sender, TaskEventArgs e)
        {
            _taskList.Add(e.ReadingTask);
        }

        protected virtual void OnSyncElementCompleted(object sender, EventArgs e)
        {
            if (ActiveSyncs.Contains(sender as ISyncElement))
                ActiveSyncs.Remove(sender as ISyncElement);

            if (CheckAllElementsComplete())
            {
                UpdateRunning = false;
                RaiseSyncTaskCompleted();
            }
        }

        protected virtual void OnSyncElementStarting(object sender, EventArgs e)
        {
            ActiveSyncs.Add(sender as ISyncElement);
        }

        protected virtual void OnSyncErrorRaised(object sender, SyncErrorEventArgs e)
        {
            CreateLogEntry("Error: " + e.ErrorMessage.Replace('\n', ' '));
        }

        protected virtual void OnSyncFailureRaised(object sender, EventArgs e)
        {
            string logMessage = "Sincronizzazione fallita: " + (sender as ISyncElement).Name;
            CreateLogEntry(logMessage);
        }

        protected virtual void RaiseNewSyncTaskStarted()
        {
            NewSyncTaskStarted?.Invoke(this, new EventArgs());
        }

        protected virtual void RaiseNewSyncTaskStarting()
        {
            NewSyncTaskStarting?.Invoke(this, new EventArgs());
        }

        protected virtual void RaiseSyncTaskCompleted()
        {
            SyncTaskCompleted?.Invoke(this, new EventArgs());
        }

        #endregion Methods
    }
}