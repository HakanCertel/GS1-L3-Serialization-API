namespace GS1L3.Application.Dtos
{
    public class ProductDto
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string? GTIN { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
    }
}
