using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMD
{
    public class InspectionCharacteristic
    {
        public InspectionCharacteristic()
        {
            InspectionOperations = new HashSet<InspectionOperation>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<InspectionOperation> InspectionOperations { get; set; }
    }
}
