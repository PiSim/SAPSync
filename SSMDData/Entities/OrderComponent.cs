namespace SSMD
{
    public partial class OrderComponent
    {
        #region Constructors

        public OrderComponent()
        {
        }

        #endregion Constructors

        #region Properties

        public Component Component { get; set; }
        public int ComponentID { get; set; }
        public int ID { get; set; }
        public Order Order { get; set; }
        public int OrderNumber { get; set; }

        #endregion Properties
    }
}