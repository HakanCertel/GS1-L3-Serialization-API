using GS1L3.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GS1L3.Domain.Entities
{
    public class SSCC : BaseEntity
    {
        public string SSCCCode { get; set; } 
        public SsccLevel Level { get; set; }  
        public Guid WorkOrderId { get; set; }
        public Guid? ParentSSCCId { get; set; }

        [NotMapped]
        public override string Code { get; set; }
        [NotMapped]
        public override string Name { get; set; }

        public WorkOrder WorkOrder { get; set; }
        public SSCC? ParentSSCC { get; set; }
        public ICollection<SSCC> ChildSSCCs { get; set; } 
        public ICollection<SerialNumber> SerialNumbers { get; set; } 
    }
}
