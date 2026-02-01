using GS1L3.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GS1L3.Domain.Entities
{
    public class WorkOrder:BaseEntity
    {
        public Guid ProductId { get; set; }
        public string LotNo { get; set; }         
        public DateTime ExpiryDate { get; set; }  
        public int TargetQuantity { get; set; }   
        public int ProducedQuantity { get; set; } 
        public string SerialStartValue { get; set; }
        public WorkOrderStatus Status { get; set; }

        [NotMapped]
        public override string Name { get; set; }
        public Product Product { get; set; }
        public ICollection<SerialNumber> SerialNumbers { get; set; }
        public ICollection<SSCC> SSCCs { get; set; }    
    }
}
