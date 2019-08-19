using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SSMD
{
    public class Customer
    {
        #region Properties

        [Key]
        public int ID { get; set; }

        public ICollection<MaterialCustomer> MaterialCustomers { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }

        #endregion Properties
    }
}