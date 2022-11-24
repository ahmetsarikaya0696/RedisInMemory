using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repository;

namespace RedisExampleApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            return Ok(await _productRepository.GetProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(Product product)
        {
            return Created(string.Empty, await _productRepository.AddAsync(product));
        }
    }
}
