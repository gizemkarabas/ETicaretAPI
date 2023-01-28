using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
		public async Task Get() 
		{
			Order order=await _orderReadRepository.GetByIdAsync("74788f8b-1951-4172-a393-7b509792a48b");
			order.Address = "İstanbul";
			await _orderWriteRepository.SaveAsync();
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			Product product= await _productReadRepository.GetByIdAsync(id);
			return Ok(product);
		}
	}	
}
