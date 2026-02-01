using GS1L3.Application.RequestParameters;

namespace GS1L3.Application.Services
{
    public interface IWorkOrderService
    {
        public Task PrepareSerialNumbersAsync(Guid workOrderId);
        public Task<object> CreateAggregationAsync(ProductionParamaeters request);

    }
}
