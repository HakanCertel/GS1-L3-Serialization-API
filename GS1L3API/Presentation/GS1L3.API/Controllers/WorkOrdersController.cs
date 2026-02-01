using GS1L3.Application.Dtos;
using GS1L3.Application.IRepositories;
using GS1L3.Application.RequestParameters;
using GS1L3.Application.Services;
using GS1L3.Domain.Entities;
using GS1L3.Domain.Enums;
using GS1L3.Infrastructure.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GS1L3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrdersController : ControllerBase
    {
        readonly IWorkOrderService _workOrderService;
        readonly IBaseRepository<WorkOrder> _workOrderRepository;
        readonly ILogger<WorkOrdersController> _logger;
        public WorkOrdersController(IBaseRepository<WorkOrder> workOrderRepository, IWorkOrderService workOrderService, ILogger<WorkOrdersController> logger)
        {
            _workOrderRepository = workOrderRepository;
            _workOrderService = workOrderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkOrderDto dto)
        {
            var workOrder = new WorkOrder
            {
                ProductId = Guid.Parse(dto.ProductId),
                LotNo = dto.LotNo,
                ExpiryDate = dto.ExpiryDate,
                TargetQuantity = dto.TargetQuantity,
                SerialStartValue=dto.SerialStartValue
            };
            workOrder.Status = dto.Status.GetEnum<WorkOrderStatus>();//WorkOrderStatus.Active

            await _workOrderRepository.AddAsync(workOrder);
            await _workOrderRepository.SaveAsync();

            await _workOrderService.PrepareSerialNumbersAsync(workOrder.Id);
            
            _logger.LogInformation($"{workOrder.Id}  Yeni bir iş emri oluşturuldu");
           
            return Ok(new { Message = "İş emri oluşturuldu ve seri numaraları rezerve edildi.", WorkOrderId = workOrder.Id });
        }

        [HttpGet("{id}/full-report")]
        public async Task<IActionResult> GetFullReport(string id)
        {
            var wo = await _workOrderRepository.Table.Where(x => x.Id.ToString() == id)
                .Include(x=>x.Product)
                .Include(x => x.SerialNumbers)
                .Include(x => x.SSCCs).ThenInclude(s => s.ChildSSCCs)
                .FirstOrDefaultAsync();

            if (wo == null) return NotFound();

            var response = new WorkOrderDetailDto
            {
                Id = wo.Id.ToString(),
                LotNo = wo.LotNo,
                ExpiryDate = wo.ExpiryDate,
                ProductName = wo.Product.Name,
                Gtin = wo.Product.GTIN,
                TargetQuantity = wo.TargetQuantity,
                ProducedQuantity = wo.ProducedQuantity,
                RemainQuantity = wo.TargetQuantity - wo.ProducedQuantity,
                Status = wo.Status.toName(),
                SerialNumbers = wo.SerialNumbers.Select(s => new SerialNumberDto
                {
                    SN = s.SN,
                    FullGs1Code = $"(01){wo.Product.GTIN}(21){wo.ExpiryDate:yyMMdd}(10){wo.LotNo}(17){s.SN}"
                }).ToList(),
                Aggregations = wo.SSCCs.Select(s => new SsccDto
                {
                    SSCCCode = s.SSCCCode,
                    Level = s.Level.ToString(),
                    ChildCodes = s.Level == SsccLevel.Case
                        ? wo.SerialNumbers.Where(sn => sn.SSCCId == s.Id).Select(sn => sn.SN).ToList()
                        : s.ChildSSCCs.Select(c => c.SSCCCode).ToList()
                }).ToList()
            };
            

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ProduceWorkOrder(ProductionParamaeters request)
        {
            request.WorkOrdere= await _workOrderRepository.GetWhere(x=>x.Id.ToString()==request.WorkOrderId&&x.Status==WorkOrderStatus.Active).FirstOrDefaultAsync();
            
            if (request.WorkOrdere == null)
                return Ok(new { Message = "İlgili iş emri için üretim tamalanmış! Lütfen farklı bir iş emri seçiniz." });
            
            var resultObj=_workOrderService.CreateAggregationAsync(request);
            
            return await GetFullReport(request.WorkOrderId);
           
            //return Ok(resultObj.Result);
        }
    }
}
