using GS1L3.Application.Dtos;
using GS1L3.Application.IRepositories;
using GS1L3.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GS1L3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly IBaseRepository<Product> _productRepository;

        public ProductController(IBaseRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Code=dto.Code,
                CustomerId=Guid.Parse(dto.CustomerId),
                GTIN=dto.GTIN,
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync();

            return Ok(product);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductDto dto)
        {

             _productRepository.Update(new()
             {
                 Id = Guid.Parse(dto.Id),
                 Code = dto.Code,
                 CustomerId = Guid.Parse(dto.CustomerId),
                 GTIN = dto.GTIN,
                 Name = dto.Name,
                 IsActive = dto.IsActive,
                 IsDeleted = dto.IsDeleted,
                 CreatedAt = Convert.ToDateTime(dto.CreatedAt),
             });
            await _productRepository.SaveAsync();

            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productRepository.RemoveAsync(id);
            await _productRepository.SaveAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var totalCount = _productRepository.GetAll(false).Count();

            var customer = _productRepository.Table.Select(x => new ProductDto
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                CustomerId=x.CustomerId.ToString(),
                GTIN = x.GTIN,
                Code = x.Code,
                CustomerName=x.Customer.Name,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                CreatedAt = Convert.ToDateTime(x.CreatedAt),
            }).ToList();

            return Ok(new { totalCount, customer });
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productRepository.Table.Where(x => x.Id.ToString() == id).Select(x => new ProductDto
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                CustomerId = x.CustomerId.ToString(),
                GTIN = x.GTIN,
                Code = x.Code,
                CustomerName = x.Customer.Name,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                CreatedAt = Convert.ToDateTime(x.CreatedAt),
            }).FirstOrDefaultAsync();

            return Ok(product);
        }
    }
}
