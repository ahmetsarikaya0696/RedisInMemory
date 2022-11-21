using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            await _distributedCache.SetStringAsync("name", "ahmet", cacheEntryOptions);
            return View();
        }

        public async Task<IActionResult> Show()
        {
            ViewBag.Name = await _distributedCache.GetStringAsync("name");
            return View();
        }

        public async Task<IActionResult> Remove()
        {
            await _distributedCache.RemoveAsync("name");
            return View();
        }
    }
}
