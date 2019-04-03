using SAP.Middleware.Connector;
using SAPSync.Functions;
using SSMD;
using DataAccessCore.Commands;
using System;
using System.Collections.Generic;

namespace SAPSync
{
    public class SyncMaterials : SyncElement
    {
        #region Constructors

        public SyncMaterials()
        {
            Name = "Materiali";
        }

        #endregion Constructors

        #region Methods

        public override void StartSync(RfcDestination destination, SSMDData sSMDData)
        {
            IRfcTable materialTable = RetrieveMaterials(destination);
            List<Material> convertedMaterials = ConvertMaterialTable(materialTable);
            PushMaterials(convertedMaterials, sSMDData);
        }

        private void PushMaterials(List<Material> materials, SSMDData sSMDData)
        {
            sSMDData.Execute(new BulkInsertEntitiesCommand<SSMDContext>(materials));
        }

        private List<Material> ConvertMaterialTable(IRfcTable materialTable)
        {
            List<Material> output = new List<Material>();

            foreach (IRfcStructure row in materialTable)
            {
                Material newMaterial = new Material();
                newMaterial.Code = row.GetString("MATERIAL");
                output.Add(newMaterial);
            }

            return output;
        }

        private IRfcTable RetrieveMaterials(RfcDestination destination)
        {
            IRfcTable output;

            try
            {
                output = new MaterialsGetList().Invoke(destination);
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