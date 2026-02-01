using Azure.Core;
using GS1L3.Application.Dtos;
using GS1L3.Application.IRepositories;
using GS1L3.Application.RequestParameters;
using GS1L3.Application.Services;
using GS1L3.Domain.Entities;
using GS1L3.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GS1L3.Persistence.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IBaseRepository<WorkOrder> _workOrderRepository;
        private readonly IBaseRepository<SerialNumber> _serialNumberRepository;
        private readonly IBaseRepository<SSCC> _ssccRepository;
        private readonly IGs1Service _gs1Service;

        public WorkOrderService(IGs1Service gs1Service, IBaseRepository<WorkOrder> workOrderRepository, IBaseRepository<SerialNumber> serialNumberRepository, IBaseRepository<SSCC> ssccRepository)
        {

            _gs1Service = gs1Service;
            _workOrderRepository = workOrderRepository;
            _serialNumberRepository = serialNumberRepository;
            _ssccRepository = ssccRepository;
        }
       /// <summary>
       /// İş Emri oluşturulurken iş emri numarasına bağlı olarak iş emri miktarı kadar seri numarası üretip veri tabanına kaydedilir
       /// </summary>
       /// <param name="workOrderId"></param>
       /// <returns></returns>
        public async Task PrepareSerialNumbersAsync(Guid workOrderId)
        {
            var wo = await _workOrderRepository.GetByIdAsync(workOrderId.ToString());
            var serials = new List<SerialNumber>();

            for (int i = 0; i < wo.TargetQuantity; i++)
            {
                serials.Add(new SerialNumber
                {
                    WorkOrderId = workOrderId,
                    SN =wo.SerialStartValue+Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(), 
                });
            }

            await _serialNumberRepository.AddRangeAsync(serials);
            await _serialNumberRepository.SaveAsync();
        }
        /// <summary>
        /// Koli ve Palet etiketleri için SSCC kodları üretilir. Koli içi  adet 10, palette 10 koli varsayılan miktarlar olarak belirlenmiştir. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> CreateAggregationAsync(ProductionParamaeters request)
        {
            List<string> gs1List = new List<string>();
            List<SerialNumber> serialNumbers = new List<SerialNumber>();
            IList<SSCC> ssccList = new List<SSCC>();
            var sb = new StringBuilder();
            
            int peaceInBox = 10;
            int boxInPallet = 10;
            
            int producedPeace = 0;
            int producedBox = 0;

            var loopCount = request.ProducedQuantity;

            var wo = await GetWorkOrderDetail(request.WorkOrderId);

            while (loopCount != 0 && loopCount != null)
            {
                var guid = Guid.Parse(request.WorkOrderId);
                SerialNumber? sn =  _serialNumberRepository.GetWhere(x=>x.WorkOrderId==guid).FirstOrDefaultAsync(x=>x.IsActive).Result;

                if (sn != null)
                {
                    var gs1 = _gs1Service.GenerateGs1String(wo.Gtin, Convert.ToString(sn.SN), wo.ExpiryDate, wo.LotNo);

                    sn.IsActive = false;

                    _serialNumberRepository.Update(sn);
                   
                    int result=_serialNumberRepository.SaveAsync().Result;
                    
                    if (result > 0)
                    {
                        sb.Append($"{sn.SN} serisi kullanılarak  {gs1.ToString()} nolu GS1 barkodu üretilmiştir.\n");

                        gs1List.Add(gs1);
                        serialNumbers.Add(sn);
                        producedPeace++;
                    }
                    else
                    {
                        sb.Append( $"Geçersiz barkod! Ürün reddedildi\n");
                    }
                    
                }

                if (producedPeace == peaceInBox)
                {
                    var sscc =  CreateCaseAggregationAsync(request.WorkOrderId, serialNumbers).Result;
                    
                    if (sscc != null)
                    {

                        sb.Append( $"{serialNumbers} serilerine ait {sscc.Code} nolu koli barkodu başarılı birşekilde oluşturulmuştur\n") ;
                        ssccList.Add(sscc);
                        producedBox++;
                        producedPeace = 0;
                    }
                    else {
                        sb.Append( $"{serialNumbers} serileri için barkod oluşturma aşamasında hata oluştu\n");
                    }
                    

                }
                if (producedBox == boxInPallet)
                {

                    await CreatePalletAggregationAsync(request.WorkOrderId, ssccList.ToList());
                    producedBox = 0;
                }

                loopCount--;

            }

            var totalProducedQuantity = request.ProducedQuantity + request.WorkOrdere?.ProducedQuantity;
            
            request.WorkOrdere.IsActive = totalProducedQuantity<=request.WorkOrdere?.TargetQuantity;
            request.WorkOrdere.ProducedQuantity = Convert.ToInt32(totalProducedQuantity);
            request.WorkOrdere.Status = !request.WorkOrdere.IsActive ? WorkOrderStatus.Completed : request.WorkOrdere.Status;
            
            _workOrderRepository.Update(request.WorkOrdere);
           
            await _workOrderRepository.SaveAsync();

            sb.Append(!request.WorkOrdere.IsActive ? "-" : $"{request.WorkOrderId} nolu iş emri tamamlanmıştır");

            return new { Message = sb.ToString(), ProducedQuantity = request.ProducedQuantity };

        }
        /// <summary>
        /// Verili iş emri 'workOrderId' ve 10 adet  seri numaralarısı/ürün için Koli SSCC üretilir
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="serialNumbers"></param>
        /// <returns></returns>
        private async Task<SSCC> CreateCaseAggregationAsync(string workOrderId, List<SerialNumber> serialNumbers)
        {
            var sscc = new SSCC
            {
                WorkOrderId = Guid.Parse(workOrderId),
                Level = SsccLevel.Case,
                SSCCCode = _gs1Service.CreateSscc("8681234", new Random().Next(100000, 999999))
            };

            await _ssccRepository.AddAsync(sscc);
            
            int result= _ssccRepository.SaveAsync().Result;
            
            if (result > 0)
            {
                foreach (var item in serialNumbers)
                {
                    item.SSCCId = sscc.Id;
                }

                _serialNumberRepository.UpdateRange(serialNumbers);
                 await _serialNumberRepository.SaveAsync();

            }
            
            serialNumbers.Clear();

            return sscc;
        }

        private async Task CreatePalletAggregationAsync(string workOrderId, List<SSCC> ssccList)
        {
            var sscc = new SSCC
            {
                WorkOrderId =Guid.Parse( workOrderId),
                Level = SsccLevel.Pallet,
                SSCCCode = _gs1Service.CreateSscc("8681234", new Random().Next(100000, 999999))
            };

            await _ssccRepository.AddAsync(sscc);

            
            foreach (var item in ssccList)
            {
                item.ParentSSCCId = sscc.Id;
            }

            _ssccRepository.UpdateRange(ssccList);
            await _ssccRepository.SaveAsync();

        }
        private async Task<WorkOrderDetailDto> GetWorkOrderDetail(string id)
        {
            WorkOrderDetailDto wo=_workOrderRepository.GetWhere(x => x.Id.ToString() == id).Select(x=>new WorkOrderDetailDto
            {
                Id=x.Id.ToString(),
                ExpiryDate=x.ExpiryDate,
                Gtin=x.Product.GTIN,
                LotNo=x.LotNo,
                ProductName=x.Product.Name,
                //SerialNumbers=x.SerialNumbers,
            }).FirstOrDefaultAsync().Result;
            return wo;
        }
    }
}
