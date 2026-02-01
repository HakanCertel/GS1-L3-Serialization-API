namespace GS1L3.Application.Dtos
{
    public class WorkOrderDto
    {
        public string? Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string ProductId { get; set; }
        public string LotNo { get; set; }         
        public DateTime ExpiryDate { get; set; }  
        public int TargetQuantity { get; set; }   
        public int ProducedQuantity { get; set; } 
        public string? SerialStartValue { get; set; } 
        public string? Status { get; set; }

    }
}
