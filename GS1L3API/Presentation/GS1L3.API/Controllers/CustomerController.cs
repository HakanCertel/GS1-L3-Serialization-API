using GS1L3.Application.Dtos;
using GS1L3.Application.IRepositories;
using GS1L3.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GS1L3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        readonly IBaseRepository<Customer> _customerRepository;
        readonly ILogger<CustomerController> _logger;
        public CustomerController(IBaseRepository<Customer> customerRepository, ILogger<CustomerController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerDto dto)
        {
            var customer = new Customer 
            {
               Description = dto.Description,
               GLN = dto.GLN,
               Name = dto.Name,
            };

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveAsync();
            
            _logger.LogInformation($"{customer.Name} Yeni bir Müşteri Oluşturuldu");
            
            return Ok(customer);
        }
        
        [HttpPut]
        public async Task<IActionResult> Update(CustomerDto dto)
        {
            var customer = 

             _customerRepository.Update(new ()
             {
                 Id = Guid.Parse(dto.Id),
                 Description = dto.Description,
                 GLN = dto.GLN,
                 Name = dto.Name,
                 IsActive=dto.IsActive,
                 IsDeleted=dto.IsDeleted,
                 CreatedAt=Convert.ToDateTime(dto.CreatedAt),
             });
            await _customerRepository.SaveAsync();

            _logger.LogInformation($"{dto.Name} Müşteri Kaydında güncelleme işlemi yapıldı.");

            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _customerRepository.RemoveAsync(id);
            await _customerRepository.SaveAsync();
            _logger.LogInformation($"{id} li kaydı silindi.");

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var totalCount = _customerRepository.GetAll(false).Count();

            var customer = _customerRepository.Table.Select(x => new CustomerDto
            {
               Id=x.Id.ToString(),
               Name=x.Name,
               IsActive=x.IsActive,
               IsDeleted = x.IsDeleted,
               CreatedAt = Convert.ToDateTime(x.CreatedAt),
               Description = x.Description,
               GLN = x.GLN,
               Products = x.Products
            }).ToList();

            return Ok(new { totalCount, customer });
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var customer = await _customerRepository.Table.Select(x => new CustomerDto
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                CreatedAt = Convert.ToDateTime(x.CreatedAt),
                Description = x.Description,
                GLN = x.GLN,
                Products = x.Products
            }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(customer);
        }
    }
}
