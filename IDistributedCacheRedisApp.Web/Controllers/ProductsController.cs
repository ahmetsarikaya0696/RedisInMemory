using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(15);

            Product product = new() { Id = 1, Name = "Kalem 1", Price = 200 };
            string jsonProduct = JsonSerializer.Serialize(product);
            await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            string jsonProduct = await _distributedCache.GetStringAsync("product:1");
            try
            {
                Product product = JsonSerializer.Deserialize<Product>(jsonProduct);
                ViewBag.Product = product;
            }
            catch (Exception)
            {
                ViewBag.Product = null;
            }

            return View();
        }

        public async Task<IActionResult> Remove()
        {
            await _distributedCache.RemoveAsync("product:1");
            return View();
        }
    }
}
