using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
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

            #region Byte Array'ine dönüştürme yöntemi
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            await _distributedCache.SetAsync("product:1", byteProduct);
            #endregion


            //await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            //string jsonProduct = await _distributedCache.GetStringAsync("product:1");
            #region Byte Array olarak alma
            byte[] byteProduct = await _distributedCache.GetAsync("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);
            #endregion
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

        public async Task<IActionResult> ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/togg.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            await _distributedCache.SetAsync("img:1", imageByte);

            return View();
        }

        public async Task<IActionResult> ImageUrl()
        {
            try
            {
                byte[] imageByte = await _distributedCache.GetAsync("img:1");
                return File(imageByte, "image/jpg");
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(ImageCache));
            }
        }
    }
}
