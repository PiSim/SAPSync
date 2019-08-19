using SAPSync.Functions;
using SAPSync.SyncElements;
using SAPSync.SyncElements.Evaluators;
using SAPSync.SyncElements.ExcelWorkbooks;
using SAPSync.SyncElements.SAPTables;
using SAPSync.SyncElements.SyncJobs;
using SAPSync.SyncElements.SyncJobs.Dto;
using SSMD;
using SSMD.Queries;
using SyncService;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                "Centri di Lavoro")
                .HasJob(
                    new SyncData<WorkCenter>(
                        new ReadWorkCenters(),
                        new RecordWriter<WorkCenter>(
                            new WorkCenterEvaluator(new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))));


            ISyncElement MaterialFamiliesElement = new JobAggregator(
                "Gerarchia Prodotto",
                new SyncElementConfiguration())
                .HasJob(new SyncData<MaterialFamilyLevel>(
                    new ReadMaterialFamilyLevels(),
                    new RecordWriter<MaterialFamilyLevel>(
                        new MaterialFamilyLevelEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))))
                .HasJob(new SyncData<MaterialFamily>(
                    new ReadMaterialFamilies(),
                    new RecordWriter<MaterialFamily>(
                        new MaterialFamilyEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))));

            ISyncElement ProjectsElement = new JobAggregator(
                "Progetti")
                .HasJob(new SyncData<Project>(
                    new ReadProjects(),
                    new RecordWriter<Project>(
                        new ProjectEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))));

            ISyncElement WBSRelationsElement = new JobAggregator(
                "Struttura progetti")
                .HasJob(new SyncData<WBSRelation>(
                    new ReadWBSRelations(),
                    new RecordWriter<WBSRelation>(
                        new WBSRelationEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    ProjectsElement
                });

            ISyncElement MaterialsElement = new JobAggregator(
                "Materiali")
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
                "Ordini")
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
                "Operazioni ordine")
                .HasJob(new SyncData<RoutingOperation>(
                    new ReadRoutingOperations(),
                    new RecordWriter<RoutingOperation>(
                        new RoutingOperationEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    WorkCentersElement
                });

            ISyncElement ComponentsElement = new JobAggregator(
                "Componenti")
                .HasJob(new SyncData<Component>(
                    new ReadComponents(),
                    new RecordWriter<Component>(
                        new ComponentEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                            IgnoreExistingRecords = true}))));

            ISyncElement ConfirmationsElements = new JobAggregator(
                "Conferme ordine")
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
                "Componenti ordine")
                .HasJob(new SyncData<OrderComponent>(
                    new ReadOrderComponents(),
                    new RecordWriter<OrderComponent>(
                        new OrderComponentEvaluator(
                            new RecordEvaluatorConfiguration()
                            {

                                IgnoreExistingRecords = true
                            }))))
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    ComponentsElement
                });

            ISyncElement InspectionCharacteristicsElement = new JobAggregator(
                "Controlli")
                .HasJob(new SyncData<InspectionCharacteristic>(
                    new ReadInspectionCharacteristics(),
                    new RecordWriter<InspectionCharacteristic>(
                        new InspectionCharacteristicEvaluator(
                            new RecordEvaluatorConfiguration()
                            {

                                IgnoreExistingRecords = true
                            }))))
                .HasJob(new SyncData<InspectionLot>(
                    new InspLotGetList(),
                    new RecordWriter<InspectionLot>(
                        new InspectionLotEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))))
                .HasJob(new SyncData<InspectionSpecification>(
                    new ReadInspectionSpecifications(),
                    new RecordWriter<InspectionSpecification>(
                        new InspectionSpecificationEvaluator(
                            new RecordEvaluatorConfiguration()
                            {

                                IgnoreExistingRecords = true
                            }))))
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
                "Clienti")
                .HasJob(new SyncData<Customer>(
                    new ReadCustomers(),
                    new RecordWriter<Customer>(
                        new CustomerEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                CheckRemovedRecords = false,
                                IgnoreExistingRecords = true
                            }))));

            ISyncElement MaterialCustomerElement = new JobAggregator(
                "Clienti per materiale")
                .HasJob(new SyncData<MaterialCustomer>(
                    new ReadMaterialCustomers(),
                    new RecordWriter<MaterialCustomer>(
                        new MaterialCustomerEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))))
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement,
                    CustomersElement
                });


            ISyncElement GoodMovementsElement = new JobAggregator(
                "Movimenti Merce")
                .HasJob(new SyncData<GoodMovement>(
                    new ReadGoodMovements(),
                    new RecordWriter<GoodMovement>(
                        new GoodMovementEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement,
                    OrdersElement
                });

            ISyncElement TrialMasterListElement = new JobAggregator(
                "Foglio Master Prove")
                .HasJob(new SyncData<OrderData>(
                    new XmlReader<OrderData, TrialMasterDataDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("L:\\LABORATORIO\\StatoOdpProva.xlsx"),
                            "Master Prove",
                            4,
                            new System.IO.DirectoryInfo("\\\\vulcaflex.locale\\datid\\Laboratorio\\LABORATORIO\\BackupReport\\StatoOdpProva"))),
                    new RecordWriter<OrderData>(
                        new TrialMasterEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                CheckRemovedRecords = false
                            }))))
                .HasJob(new SyncData<OrderData>(
                    new SSMDReader<OrderData>(() => new LoadedOrderDataQuery()),
                    new XmlWriter<OrderData, TrialMasterDataDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("L:\\LABORATORIO\\StatoOdpProva.xlsx"),
                            "Master Prove",
                            4,
                            new System.IO.DirectoryInfo("L:\\LABORATORIO\\BackupReport\\ODPProva"))
                        {
                            ImportedColumnFill = Color.Yellow
                        })))
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    OrderComponentsElement
                });

            ISyncElement TrialLabDataElement = new JobAggregator(
                "Foglio note fasi di lavorazione")
                .HasJob(new SyncData<WorkPhaseLabData>(
                    new XmlReader<WorkPhaseLabData, WorkPhaseLabDataDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("\\\\vulcaflex.locale\\datid\\Laboratorio\\LABORATORIO\\ODPProva.xlsx"),
                            "SCHEMA",
                            4,
                            new System.IO.DirectoryInfo("L:\\LABORATORIO\\BackupReport\\ODPProva"))),
                    new RecordWriter<WorkPhaseLabData>(
                        new WorkPhaseLabDataEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                CheckRemovedRecords = false
                            }))))
                .HasJob(new CreateMissingTrialLabData())
                .HasJob(new SyncData<WorkPhaseLabData>(
                    new SSMDReader<WorkPhaseLabData>(() => new LoadedWorkPhaseLabDataQuery()),
                    new XmlWriter<WorkPhaseLabData, WorkPhaseLabDataDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("\\\\vulcaflex.locale\\datid\\Laboratorio\\LABORATORIO\\ODPProva.xlsx"),
                            "SCHEMA",
                            4,
                            new System.IO.DirectoryInfo("L:\\LABORATORIO\\BackupReport\\ODPProva"))
                        {
                            ImportedColumnFill = Color.Yellow
                        })))
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    TrialMasterListElement
                });
            
            ISyncElement TestReportElement = new JobAggregator(
                "Test Report")
                .HasJob(new SyncData<TestReport>(
                    new XmlReader<TestReport, TestReportDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("L:\\LABORATORIO\\ListaReport.xlsx"), 
                            "Report",
                            4)),
                    new RecordWriter<TestReport>(
                        new TestReportRecordEvaluator())))
                .HasJob(new SyncData<TestReport>(
                    new SSMDReader<TestReport>( () => new LoadedTestReportQuery()),
                    new XmlWriter<TestReport, TestReportDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("L:\\LABORATORIO\\ListaReport.xlsx"),
                            "Report",
                            4,
                            new System.IO.DirectoryInfo("L:\\LABORATORIO\\BackupReport\\ListaReport"))
                        {
                            ImportedColumnFill = Color.Yellow
                        })))
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    TrialMasterListElement
                });

            ISyncElement TrialScrapListElement = new JobAggregator(
                "Foglio scarti di prova")
                .HasJob(new SyncData<IGrouping<Tuple<Order, string>, OrderConfirmation>>(
                    new SSMDReader<OrderConfirmation, IGrouping<Tuple<Order, string>, OrderConfirmation>>(
                        qu => qu.GroupBy(con => new Tuple<Order, string>(con.Order, con.ScrapCause))
                            .ToList()
                            .AsQueryable()
                            .OrderByDescending(grp => grp.Key.Item1.Number),
                        () => new LoadedOrderScrapCauseConfirmations()),
                    new XmlWriter<IGrouping<Tuple<Order, string>, OrderConfirmation>, TrialScrapDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("W:\\Bacheca\\Qualita Pubblica\\Scarti\\ScartoOdp.xlsx"),
                            "Scarti",
                            4,
                            new System.IO.DirectoryInfo("L:\\temp\\Pietro"))
                        {
                            ImportedColumnFill = Color.Yellow
                        })))
                .DependsOn(new ISyncElement[]
                {
                    ConfirmationsElements,
                    OrdersElement
                });

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
                    TestReportElement,
                    TrialScrapListElement
                };

            return output;
        }
    }
}
