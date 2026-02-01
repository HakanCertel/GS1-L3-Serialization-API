namespace GS1L3.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public virtual string? Code { get; set; }
        public virtual string Name { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } 
        public bool IsDeleted { get; set; } 
        public bool IsActive { get; set; } 
    }
}
