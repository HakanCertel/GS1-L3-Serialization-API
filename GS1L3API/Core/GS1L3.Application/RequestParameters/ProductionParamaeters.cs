using GS1L3.Domain.Entities;

namespace GS1L3.Application.RequestParameters
{
    public class ProductionParamaeters
    {
        public string WorkOrderId { get; set; }
        public int ProducedQuantity { get; set; }
        public WorkOrder? WorkOrdere { get; set; }
    }
}
