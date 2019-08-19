using SAPSync.Functions;
using SAPSync.SyncElements;
using SAPSync.SyncElements.Evaluators;
using SAPSync.SyncElements.ExcelWorkbooks;
using SAPSync.SyncElements.SAPTables;
using SAPSync.SyncElements.SyncJobs;
using SSMD;
using SSMD.Queries;
using SyncService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync
{
    public class SyncElementFactory
    {
        public virtual ICollection<ISyncElement> BuildSyncElements()
        {
            ISyncElement WorkCentersElement = new JobAggregator(
                "Centri di Lavoro",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(
                    new SyncData<WorkCenter>(
                        new ReadWorkCenters(),
                        new RecordWriter<WorkCenter>(
                            new WorkCenterEvaluator())));


            ISyncElement MaterialFamiliesElement = new JobAggregator(
                "Gerarchia Prodotto",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<MaterialFamilyLevel>(
                    new ReadMaterialFamilyLevels(),
                    new RecordWriter<MaterialFamilyLevel>(
                        new MaterialFamilyLevelEvaluator())))
                .HasJob(new SyncData<MaterialFamily>(
                    new ReadMaterialFamilies(),
                    new RecordWriter<MaterialFamily>(
                        new MaterialFamilyEvaluator())));

            ISyncElement ProjectsElement = new JobAggregator(
                "Progetti",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<Project>(
                    new ReadProjects(),
                    new RecordWriter<Project>(
                        new ProjectEvaluator())));

            ISyncElement WBSRelationsElement = new JobAggregator(
                "Struttura progetti",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<WBSRelation>(
                    new ReadWBSRelations(),
                    new RecordWriter<WBSRelation>(
                        new WBSRelationEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    ProjectsElement
                });

            ISyncElement MaterialsElement = new JobAggregator(
                "Materiali",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<Material>(
                    new ReadMaterials(),
                    new RecordWriter<Material>(
                        new MaterialEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    MaterialFamiliesElement,
                    ProjectsElement
                });

            ISyncElement OrdersElement = new JobAggregator(
                "Ordini",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<Order>(
                    new ReadOrders(),
                    new RecordWriter<Order>(
                        new OrderEvaluator())))
                .HasJob(new SyncData<OrderData>(
                    new ReadOrderData(),
                    new RecordWriter<OrderData>(
                        new OrderDataEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement
                });

            ISyncElement RoutingOperationsElement = new JobAggregator(
                "Operazioni ordine",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<RoutingOperation>(
                    new ReadRoutingOperations(),
                    new RecordWriter<RoutingOperation>(
                        new RoutingOperationEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    WorkCentersElement
                });

            ISyncElement ComponentsElement = new JobAggregator(
                "Componenti",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<Component>(
                    new ReadComponents(),
                    new RecordWriter<Component>(
                        new ComponentEvaluator())));

            ISyncElement ConfirmationsElements = new JobAggregator(
                "Conferme ordine",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<OrderConfirmation>(
                    new ReadConfirmations(),
                    new RecordWriter<OrderConfirmation>(
                        new ConfirmationEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    WorkCentersElement
                });

            ISyncElement OrderComponentsElement = new JobAggregator(
                "Componenti ordine",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<OrderComponent>(
                    new ReadOrderComponents(),
                    new RecordWriter<OrderComponent>(
                        new OrderComponentEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    ComponentsElement
                });

            ISyncElement InspectionCharacteristicsElement = new JobAggregator(
                "Controlli",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<InspectionCharacteristic>(
                    new ReadInspectionCharacteristics(),
                    new RecordWriter<InspectionCharacteristic>(
                        new InspectionCharacteristicEvaluator())))
                .HasJob(new SyncData<InspectionLot>(
                    new InspLotGetList(),
                    new RecordWriter<InspectionLot>(
                        new InspectionLotEvaluator())))
                .HasJob(new SyncData<InspectionSpecification>(
                    new ReadInspectionSpecifications(),
                    new RecordWriter<InspectionSpecification>(
                        new InspectionSpecificationEvaluator())))
                .HasJob(new SyncData<InspectionPoint>(
                    new ReadInspectionPoints(),
                    new RecordWriter<InspectionPoint>(
                        new InspectionPointEvaluator())))
                .DependsOn(
                    new ISyncElement[]
                    {
                        OrdersElement
                    });

            ISyncElement CustomersElement = new JobAggregator(
                "Clienti",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = false,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<Customer>(
                    new ReadCustomers(),
                    new RecordWriter<Customer>(
                        new CustomerEvaluator())));

            ISyncElement MaterialCustomerElement = new JobAggregator(
                "Clienti per materiale",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = true,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<MaterialCustomer>(
                    new ReadMaterialCustomers(),
                    new RecordWriter<MaterialCustomer>(
                        new MaterialCustomerEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement,
                    CustomersElement
                });


            ISyncElement GoodMovementsElement = new JobAggregator(
                "Movimenti Merce",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = false,
                    PerformImport = true
                })
                .HasJob(new SyncData<GoodMovement>(
                    new ReadGoodMovements(),
                    new RecordWriter<GoodMovement>(
                        new GoodMovementEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement,
                    OrdersElement
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
                    OrdersElement
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
                    TrialMasterListElement
                });

            ISyncElement XmlJobTestElement = new JobAggregator(
                "XMLJob Test",
                new SyncElementConfiguration()
                {
                    CheckDeletedElements = true,
                    IgnoreExistingRecords = false,
                    PerformExport = true,
                    PerformImport = true
                })
                .HasJob(new SyncData<TestReport>(
                    new XmlReader<TestReport, TestReportDto>(
                        new System.IO.FileInfo("L:\\LABORATORIO\\ListaReport.xlsx")),
                    new RecordWriter<TestReport>(
                        new TestReportRecordEvaluator())))
                .HasJob(new SyncData<TestReport>(
                    new SSMDReader<TestReport>( () => new LoadedTestReportQuery()),
                    new XmlWriter<TestReport, TestReportDto>(
                        new XmlWriterConfiguration(
                            new System.IO.FileInfo("L:\\temp\\Pietro\\ListaReport.xlsx"),
                            new System.IO.DirectoryInfo("L:\\temp\\Pietro")))));

            List<ISyncElement> output = new List<ISyncElement>
                {
                    WorkCentersElement,
                    CustomersElement,
                    MaterialFamiliesElement,
                    MaterialCustomerElement,
                    ProjectsElement,
                    WBSRelationsElement,
                    MaterialsElement,
                    OrdersElement,
                    ComponentsElement,
                    RoutingOperationsElement,
                    OrderComponentsElement,
                    ConfirmationsElements,
                    GoodMovementsElement,
                    InspectionCharacteristicsElement,
                    TrialMasterListElement,
                    TrialLabDataElement,
                    TestReportsElement,
                    XmlJobTestElement
                };

            return output;
        }
    }
}
