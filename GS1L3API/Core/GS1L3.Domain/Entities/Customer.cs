namespace GS1L3.Domain.Entities
{
    public class Customer:BaseEntity
    {
        public string GLN { get; set; } // Global Location Number (13 haneli)
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
