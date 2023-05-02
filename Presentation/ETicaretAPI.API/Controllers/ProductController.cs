using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		readonly private IProductWriteRepository _productWriteRepository;
		readonly private IProductReadRepository _productReadRepository;

		readonly private IOrderWriteRepository _orderWriteRepository;
		readonly private IOrderReadRepository _orderReadRepository;

		readonly private ICustomerWriteRepository _customerWriteRepository;

		public ProductController(
			IProductWriteRepository productWriteRepository,
			IProductReadRepository productReadRepository,
			IOrderWriteRepository orderWriteRepository,
			ICustomerWriteRepository customerWriteRepository,
			IOrderReadRepository orderReadRepository)
		{
			_productWriteRepository = productWriteRepository;
			_productReadRepository = productReadRepository;
			_orderWriteRepository = orderWriteRepository;
			_customerWriteRepository = customerWriteRepository;
			_orderReadRepository = orderReadRepository;
		}
		[HttpGet]

		public async Task<IActionResult> Get([FromQuery]Pagination pagination) 
		{
			var products =_productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
			{
				p.Id,
				p.Name,
				p.Stock,
				p.Price,
				p.CreatedDate,
				p.UpdateDate
			}).ToList();
			return Ok();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			Product product= await _productReadRepository.GetByIdAsync(id, false); //Daha performanslı bir çalışma için tracking edilmesin diyoruz.
			return Ok(product);
		}
		[HttpPost]
		public async Task<IActionResult> Post(VM_ProductCreate model)
		{
			await _productWriteRepository.AddAsync(new()
			{
				Name = model.Name,
				Price = model.Price,
				Stock = model.Stock
			});
			await _productWriteRepository.SaveAsync();

			return StatusCode((int)HttpStatusCode.Created);

		}

		[HttpPut]
		public async Task<IActionResult> Put(VM_Product_Update model)
		{
			Product product=await _productReadRepository.GetByIdAsync(model.Id);
			product.Stock = model.Stock;
			product.Name = model.Name;
			product.Price = model.Price;
			await _productWriteRepository.SaveAsync();
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			await _productWriteRepository.RemoveAsync(id);
			await _productWriteRepository.SaveAsync();
			return Ok();
		}
	}	
}
