using GS1L3.Domain.Entities;

namespace GS1L3.Application.Dtos
{
    public class WorkOrderDetailDto
    {
        public string Id { get; set; }
        public string LotNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }
        public string ProductName { get; set; }
        public string Gtin { get; set; }
        public int TargetQuantity { get; set; }
        public int? ProducedQuantity { get; set; }
        public int? RemainQuantity { get; set; }
        public ICollection<SerialNumberDto> SerialNumbers { get; set; }
        public ICollection<SsccDto> Aggregations { get; set; }
    }

    

   
}
