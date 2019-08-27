using SAPSync.RFCFunctions;
using SAPSync.SyncElements;
using SAPSync.SyncElements.Evaluators;
using SAPSync.SyncElements.ExcelWorkbooks;
using SAPSync.SyncElements.SAPTables;
using SAPSync.SyncElements.SyncOperations;
using SAPSync.SyncElements.SyncOperations.Dto;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SAPSync
{
    public class SyncElementFactory
    {
        #region Methods

        public virtual ICollection<ISyncElement> BuildSyncElements()
        {
            ISyncElement WorkCentersElement = new OperationAggregator(
                "Centri di Lavoro")
                .HasOperation(
                    new SyncData<WorkCenter>(
                        new ReadWorkCenters(),
                        new RecordWriter<WorkCenter>(
                            new WorkCenterEvaluator(new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))));

            ISyncElement MaterialFamiliesElement = new OperationAggregator(
                "Gerarchia Prodotto",
                new SyncElementConfiguration())
                .HasOperation(new SyncData<MaterialFamilyLevel>(
                    new ReadMaterialFamilyLevels(),
                    new RecordWriter<MaterialFamilyLevel>(
                        new MaterialFamilyLevelEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))))
                .HasOperation(new SyncData<MaterialFamily>(
                    new ReadMaterialFamilies(),
                    new RecordWriter<MaterialFamily>(
                        new MaterialFamilyEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))));

            ISyncElement ProjectsElement = new OperationAggregator(
                "Progetti")
                .HasOperation(new SyncData<Project>(
                    new ReadProjects(),
                    new RecordWriter<Project>(
                        new ProjectEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))));

            ISyncElement WBSRelationsElement = new OperationAggregator(
                "Struttura progetti")
                .HasOperation(new SyncData<WBSRelation>(
                    new ReadWBSRelations(),
                    new RecordWriter<WBSRelation>(
                        new WBSRelationEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    ProjectsElement
                });

            ISyncElement MaterialsElement = new OperationAggregator(
                "Materiali")
                .HasOperation(new SyncData<Material>(
                    new ReadMaterials(),
                    new RecordWriter<Material>(
                        new MaterialEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    MaterialFamiliesElement,
                    ProjectsElement
                });

            ISyncElement OrdersElement = new OperationAggregator(
                "Ordini")
                .HasOperation(new SyncData<Order>(
                    new ReadOrders(),
                    new RecordWriter<Order>(
                        new OrderEvaluator())))
                .HasOperation(new SyncData<OrderData>(
                    new ReadOrderData(),
                    new RecordWriter<OrderData>(
                        new OrderDataEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement
                });

            ISyncElement RoutingOperationsElement = new OperationAggregator(
                "Operazioni ordine")
                .HasOperation(new SyncData<RoutingOperation>(
                    new ReadRoutingOperations(),
                    new RecordWriter<RoutingOperation>(
                        new RoutingOperationEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    WorkCentersElement
                });

            ISyncElement ComponentsElement = new OperationAggregator(
                "Componenti")
                .HasOperation(new SyncData<Component>(
                    new ReadComponents(),
                    new RecordWriter<Component>(
                        new ComponentEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))));

            ISyncElement ConfirmationsElements = new OperationAggregator(
                "Conferme ordine")
                .HasOperation(new SyncData<OrderConfirmation>(
                    new ReadConfirmations(),
                    new RecordWriter<OrderConfirmation>(
                        new ConfirmationEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    WorkCentersElement
                });

            ISyncElement OrderComponentsElement = new OperationAggregator(
                "Componenti ordine")
                .HasOperation(new SyncData<OrderComponent>(
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

            ISyncElement InspectionCharacteristicsElement = new OperationAggregator(
                "Controlli")
                .HasOperation(new SyncData<InspectionCharacteristic>(
                    new ReadInspectionCharacteristics(),
                    new RecordWriter<InspectionCharacteristic>(
                        new InspectionCharacteristicEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))))
                .HasOperation(new SyncData<InspectionLot>(
                    new InspLotGetList(),
                    new RecordWriter<InspectionLot>(
                        new InspectionLotEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))))
                .HasOperation(new SyncData<InspectionSpecification>(
                    new ReadInspectionSpecifications(),
                    new RecordWriter<InspectionSpecification>(
                        new InspectionSpecificationEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }))))
                .HasOperation(new SyncData<InspectionPoint>(
                    new ReadInspectionPoints(),
                    new RecordWriter<InspectionPoint>(
                        new InspectionPointEvaluator())))
                .DependsOn(
                    new ISyncElement[]
                    {
                        OrdersElement
                    });

            ISyncElement CustomersElement = new OperationAggregator(
                "Clienti")
                .HasOperation(new SyncData<Customer>(
                    new ReadCustomers(),
                    new RecordWriter<Customer>(
                        new CustomerEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                CheckRemovedRecords = false,
                                IgnoreExistingRecords = true
                            }))));

            ISyncElement MaterialCustomerElement = new OperationAggregator(
                "Clienti per materiale")
                .HasOperation(new SyncData<MaterialCustomer>(
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

            ISyncElement GoodMovementsElement = new OperationAggregator(
                "Movimenti Merce")
                .HasOperation(new SyncData<GoodMovement>(
                    new ReadGoodMovements(),
                    new RecordWriter<GoodMovement>(
                        new GoodMovementEvaluator())))
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement,
                    OrdersElement
                });

            ISyncElement TrialMasterListElement = new OperationAggregator(
                "Foglio Master Prove")
                .HasOperation(new SyncData<OrderData>(
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
                .HasOperation(new SyncData<OrderData>(
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

            ISyncElement TrialLabDataElement = new OperationAggregator(
                "Foglio note fasi di lavorazione")
                .HasOperation(new SyncData<WorkPhaseLabData>(
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
                .HasOperation(new CreateMissingTrialLabData())
                .HasOperation(new SyncData<WorkPhaseLabData>(
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

            ISyncElement TestReportElement = new OperationAggregator(
                "Test Report")
                .HasOperation(new SyncData<TestReport>(
                    new XmlReader<TestReport, TestReportDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("L:\\LABORATORIO\\ListaReport.xlsx"),
                            "Report",
                            4)),
                    new RecordWriter<TestReport>(
                        new TestReportRecordEvaluator())))
                .HasOperation(new SyncData<TestReport>(
                    new SSMDReader<TestReport>(() => new LoadedTestReportQuery()),
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

            ISyncElement TrialScrapListElement = new OperationAggregator(
                "Foglio scarti di prova")
                .HasOperation(new SyncData<IGrouping<Tuple<Order, string>, OrderConfirmation>>(
                    new SSMDReader<OrderConfirmation, IGrouping<Tuple<Order, string>, OrderConfirmation>>(
                        qu => qu.GroupBy(con => new Tuple<Order, string>(con.Order, con.ScrapCause))
                            .ToList()
                            .AsQueryable()
                            .OrderByDescending(grp => grp.Key.Item1.Number),
                        () => new LoadedOrderScrapCauseConfirmationsQuery()),
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

        #endregion Methods
    }
}