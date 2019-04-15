using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using SSMD.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAPSync.SyncElements
{
    public class SyncMaterials : SyncElement<Material>
    {
        #region Fields

        private IDictionary<string, Material> _materialDictionary;

        #endregion Fields

        #region Constructors

        public SyncMaterials()
        {
            Name = "Materiali";
        }

        #endregion Constructors

        #region Methods

        protected override void Initialize()
        {
            base.Initialize();
            _materialDictionary = _sSMDData.RunQuery(new MaterialsQuery()).ToDictionary(mat => mat.Code, mat => mat);

            if (_materialDictionary == null)
                throw new InvalidOperationException("Errore nel recupero del dizionario Materiali");
        }

        protected override void RetrieveSAPRecords()
        {
            base.RetrieveSAPRecords();

            IRfcTable materialTable = RetrieveMaterials();
            ConvertMaterialTable(materialTable);
        }

        private void ConvertMaterialTable(IRfcTable materialTable)
        {
            _recordsToInsert = new List<Material>();

            foreach (IRfcStructure row in materialTable)
            {
                string currentMaterialCode = row.GetString("MATERIAL");

                if (_materialDictionary.ContainsKey(currentMaterialCode))
                    continue;

                Material newMaterial = new Material();
                newMaterial.Code = currentMaterialCode;
                _recordsToInsert.Add(newMaterial);
            }
        }

        private IRfcTable RetrieveMaterials()
        {
            IRfcTable output;

            try
            {
                output = new MaterialsGetList().Invoke(_rfcDestination);
            }
            catch (Exception e)
            {
                throw new Exception("RetrieveMaterials error: " + e.Message);
            }

            return output;
        }

        #endregion Methods
    }
}