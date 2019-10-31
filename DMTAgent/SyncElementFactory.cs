using DataAccessCore;
using DMTAgent.Infrastructure;
using DMTAgent.SAP;
using DMTAgent.SyncElements;
using DMTAgent.SyncElements.Evaluators;
using DMTAgent.SyncElements.ExcelWorkbooks;
using DMTAgent.SyncElements.SAPTables;
using DMTAgent.SyncElements.SyncOperations;
using DMTAgent.XML;
using Microsoft.Extensions.Logging;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DMTAgent
{
    public class SyncElementFactory : ISyncElementFactory
    {
        #region Fields

        private readonly IDataService<SSMDContext> _dataService;
        private readonly ILogger<SyncElementFactory> _logger;

        #endregion Fields

        #region Constructors

        public SyncElementFactory(IDataService<SSMDContext> dataService,
            ILogger<SyncElementFactory> logger)
        {
            _logger = logger;
            _dataService = dataService;
        }

        #endregion Constructors

        #region Methods

        public virtual ICollection<ISyncElement> BuildSyncElements()
        {
            _logger.LogInformation("Costruzione lista elementi");

            ISyncElement WorkCentersElement = new OperationAggregator(
                _dataService,
                "Centri di Lavoro")
                .HasOperation(
                    new SyncData<WorkCenter>(
                        new ReadWorkCenters(),
                        new RecordWriter<WorkCenter>(
                            new WorkCenterEvaluator(new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)));

            ISyncElement MaterialFamiliesElement = new OperationAggregator(
                _dataService,
                "Gerarchia Prodotto",
                new SyncElementConfiguration())
                .HasOperation(new SyncData<MaterialFamilyLevel>(
                    new ReadMaterialFamilyLevels(),
                    new RecordWriter<MaterialFamilyLevel>(
                        new MaterialFamilyLevelEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)))
                .HasOperation(new SyncData<MaterialFamily>(
                    new ReadMaterialFamilies(),
                    new RecordWriter<MaterialFamily>(
                        new MaterialFamilyEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)));

            ISyncElement ProjectsElement = new OperationAggregator(
                _dataService,
                "Progetti")
                .HasOperation(new SyncData<Project>(
                    new ReadProjects(),
                    new RecordWriter<Project>(
                        new ProjectEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)));

            ISyncElement WBSRelationsElement = new OperationAggregator(
                _dataService,
                "Struttura progetti")
                .HasOperation(new SyncData<WBSRelation>(
                    new ReadWBSRelations(),
                    new RecordWriter<WBSRelation>(
                        new WBSRelationEvaluator(),
                            _dataService)))
                .DependsOn(new ISyncElement[]
                {
                    ProjectsElement
                });

            ISyncElement MaterialsElement = new OperationAggregator(
                _dataService,
                "Materiali")
                .HasOperation(new SyncData<Material>(
                    new ReadMaterials(),
                    new RecordWriter<Material>(
                        new MaterialEvaluator(),
                            _dataService)))
                .DependsOn(new ISyncElement[]
                {
                    MaterialFamiliesElement,
                    ProjectsElement
                });

            ISyncElement OrdersElement = new OperationAggregator(
                _dataService,
                "Ordini")
                .HasOperation(new SyncData<Order>(
                    new ReadOrders(),
                    new RecordWriter<Order>(
                        new OrderEvaluator(),
                            _dataService)))
                .HasOperation(new SyncData<OrderData>(
                    new ReadOrderData(),
                    new RecordWriter<OrderData>(
                        new OrderDataEvaluator(),
                            _dataService)))
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement
                });

            //ISyncElement RoutingOperationsElement = new OperationAggregator(
            //    _dataService,
            //    "Operazioni ordine")
            //    .HasOperation(new SyncData<RoutingOperation>(
            //        new ReadRoutingOperations(),
            //        new RecordWriter<RoutingOperation>(
            //            new RoutingOperationEvaluator(),
            //                _dataService)))
            //    .DependsOn(new ISyncElement[]
            //    {
            //        WorkCentersElement
            //    });

            ISyncElement ComponentsElement = new OperationAggregator(
                _dataService,
                "Componenti")
                .HasOperation(new SyncData<Component>(
                    new ReadComponents(),
                    new RecordWriter<Component>(
                        new ComponentEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)));

            ISyncElement ConfirmationsElements = new OperationAggregator(
                _dataService,
                "Conferme ordine")
                .HasOperation(new SyncData<OrderConfirmation>(
                    new ReadConfirmations(),
                    new RecordWriter<OrderConfirmation>(
                        new ConfirmationEvaluator(),
                            _dataService)))
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    WorkCentersElement
                });

            ISyncElement OrderComponentsElement = new OperationAggregator(
                _dataService,
                "Componenti ordine")
                .HasOperation(new SyncData<OrderComponent>(
                    new ReadOrderComponents(),
                    new RecordWriter<OrderComponent>(
                        new OrderComponentEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)))
                .DependsOn(new ISyncElement[]
                {
                    OrdersElement,
                    ComponentsElement
                });

            ISyncElement InspectionCharacteristicsElement = new OperationAggregator(
                _dataService,
                "Controlli")
                .HasOperation(new SyncData<InspectionCharacteristic>(
                    new ReadInspectionCharacteristics(),
                    new RecordWriter<InspectionCharacteristic>(
                        new InspectionCharacteristicEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)))
                .HasOperation(new SyncData<InspectionLot>(
                    new InspLotGetList(),
                    new RecordWriter<InspectionLot>(
                        new InspectionLotEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)))
                .HasOperation(new SyncData<InspectionSpecification>(
                    new ReadInspectionSpecifications(),
                    new RecordWriter<InspectionSpecification>(
                        new InspectionSpecificationEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)))
                .HasOperation(new SyncData<InspectionPoint>(
                    new ReadInspectionPoints(),
                    new RecordWriter<InspectionPoint>(
                        new InspectionPointEvaluator(),
                            _dataService)))
                .DependsOn(
                    new ISyncElement[]
                    {
                        OrdersElement
                    });

            ISyncElement CustomersElement = new OperationAggregator(
                _dataService,
                "Clienti")
                .HasOperation(new SyncData<Customer>(
                    new ReadCustomers(),
                    new RecordWriter<Customer>(
                        new CustomerEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)));

            ISyncElement MaterialCustomerElement = new OperationAggregator(
                _dataService,
                "Clienti per materiale")
                .HasOperation(new SyncData<MaterialCustomer>(
                    new ReadMaterialCustomers(),
                    new RecordWriter<MaterialCustomer>(
                        new MaterialCustomerEvaluator(
                            new RecordEvaluatorConfiguration()
                            {
                                IgnoreExistingRecords = true
                            }),
                            _dataService)))
                .DependsOn(new ISyncElement[]
                {
                    MaterialsElement,
                    CustomersElement
                });

            //ISyncElement GoodMovementsElement = new OperationAggregator(
            //    _dataService,
            //    "Movimenti Merce")
            //    .HasOperation(new SyncData<GoodMovement>(
            //        new ReadGoodMovements(),
            //        new RecordWriter<GoodMovement>(
            //            new GoodMovementEvaluator(),
            //                _dataService)))
            //    .DependsOn(new ISyncElement[]
            //    {
            //        MaterialsElement,
            //        OrdersElement
            //    });

            ISyncElement TrialMasterListElement = new OperationAggregator(
                _dataService,
                "Foglio Master Prove")
                .HasOperation(new SyncData<OrderData>(
                    new XmlReader<OrderData, TrialMasterDataDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("L:\\LABORATORIO\\StatoOdpProva.xlsx"),
                            "Master Prove",
                            4,
                            new System.IO.DirectoryInfo("\\\\vulcaflex.locale\\datid\\Laboratorio\\LABORATORIO\\BackupReport\\StatoOdpProva"))),
                    new RecordWriter<OrderData>(
                        new TrialMasterEvaluator(),
                            _dataService)))
                .HasOperation(new SyncData<OrderData>(
                    new SSMDReader<OrderData>(_dataService,
                    () => new LoadedOrderDataQuery()),
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

            //ISyncElement TrialLabDataElement = new OperationAggregator(
            //    _dataService,
            //    "Foglio note fasi di lavorazione")
            //    .HasOperation(new SyncData<WorkPhaseLabData>(
            //        new XmlReader<WorkPhaseLabData, WorkPhaseLabDataDto>(
            //            new XmlInteractionConfiguration(
            //                new System.IO.FileInfo("\\\\vulcaflex.locale\\datid\\Laboratorio\\LABORATORIO\\ODPProva.xlsx"),
            //                "SCHEMA",
            //                4,
            //                new System.IO.DirectoryInfo("L:\\LABORATORIO\\BackupReport\\ODPProva"))),
            //        new RecordWriter<WorkPhaseLabData>(
            //            new WorkPhaseLabDataEvaluator(),
            //                _dataService)))
            //    .HasOperation(new CreateMissingTrialLabData(_dataService))
            //    .HasOperation(new SyncData<WorkPhaseLabData>(
            //        new SSMDReader<WorkPhaseLabData>(_dataService,
            //        () => new LoadedWorkPhaseLabDataQuery()),
            //        new XmlWriter<WorkPhaseLabData, WorkPhaseLabDataDto>(
            //            new XmlInteractionConfiguration(
            //                new System.IO.FileInfo("\\\\vulcaflex.locale\\datid\\Laboratorio\\LABORATORIO\\ODPProva.xlsx"),
            //                "SCHEMA",
            //                4,
            //                new System.IO.DirectoryInfo("L:\\LABORATORIO\\BackupReport\\ODPProva"))
            //            {
            //                ImportedColumnFill = Color.Yellow
            //            })))
            //    .DependsOn(new ISyncElement[]
            //    {
            //        OrdersElement,
            //        TrialMasterListElement
            //    });

            ISyncElement TestReportElement = new OperationAggregator(
                _dataService,
                "Test Report")
                .HasOperation(new SyncData<TestReport>(
                    new XmlReader<TestReport, TestReportDto>(
                        new XmlInteractionConfiguration(
                            new System.IO.FileInfo("L:\\LABORATORIO\\ListaReport.xlsx"),
                            "Report",
                            4)),
                    new RecordWriter<TestReport>(
                        new TestReportRecordEvaluator(),
                            _dataService)))
                .HasOperation(new SyncData<TestReport>(
                    new SSMDReader<TestReport>(_dataService,
                    () => new LoadedTestReportQuery()),
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
                _dataService,
                "Foglio scarti di prova")
                .HasOperation(new SyncData<IGrouping<Tuple<Order, string>, OrderConfirmation>>(
                    new SSMDReader<OrderConfirmation, IGrouping<Tuple<Order, string>, OrderConfirmation>>(
                        _dataService,
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
                    OrderComponentsElement,
                    ConfirmationsElements,
                    InspectionCharacteristicsElement,
                    TrialMasterListElement,
                    TestReportElement,
                    TrialScrapListElement
                };

            return output;
        }

        #endregion Methods
    }
}