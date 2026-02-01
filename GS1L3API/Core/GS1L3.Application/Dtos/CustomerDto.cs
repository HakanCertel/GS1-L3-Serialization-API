using GS1L3.Domain.Entities;

namespace GS1L3.Application.Dtos
{
    public class CustomerDto
    {
        public string? Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string GLN { get; set; }
        public string Description { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
