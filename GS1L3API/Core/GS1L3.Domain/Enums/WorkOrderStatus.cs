using System.ComponentModel;

namespace GS1L3.Domain.Enums
{
    public enum WorkOrderStatus:byte
    {
        [Description("Tasarım Aşamasında")]
        Draft = 0,      
        [Description("İşlemede")]
        Active = 1,     
        [Description("Tamamlandı")]
        Completed = 2,  
        [Description("İptal Edildi")]
        Cancelled = 3 
    }
}
