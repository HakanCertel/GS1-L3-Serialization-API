namespace GS1L3.Domain.Entities
{
    public class Product:BaseEntity
    {
        public string GTIN { get; set; } // Global Trade Item Number (14 haneli)
        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }
        public ICollection<WorkOrder> WorkOrders { get; set; }
    }
}
