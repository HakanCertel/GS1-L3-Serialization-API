using System.ComponentModel.DataAnnotations.Schema;

namespace GS1L3.Domain.Entities
{
    public class SerialNumber : BaseEntity
    {
        public string SN { get; set; } // AI (21) - Eşsiz olmalı
        public Guid WorkOrderId { get; set; }
        public Guid? SSCCId { get; set; }
        [NotMapped]
        public override string Code { get; set; }
        [NotMapped]
        public override string Name { get; set; }
        
        public WorkOrder WorkOrder { get; set; }
        public SSCC? SSCC { get; set; }
    }
}
