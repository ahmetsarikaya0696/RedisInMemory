using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            #region 1.Yol
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());
            #endregion

            #region 2.Yol
            #region Remove
            // time key'i ile tanımlanan cachelenmiş datayı siler
            _memoryCache.Remove("time");
            #endregion

            MemoryCacheEntryOptions options = new();
            //options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            options.SlidingExpiration = TimeSpan.FromSeconds(10);

            options.Priority = CacheItemPriority.High;

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"Key:{key}\r\nValue:{value}\r\nReason:{reason}");
            });

            _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);

            Product p = new() { Id = 1, Name = "Kalem", Price = 200 };
            _memoryCache.Set<Product>("product:1", p);
            #endregion
            return View();
        }

        public IActionResult Show()
        {

            #region GetOrCreate
            // time key'ine sahip bir veri olup olmadığını kontrol eder yoksa oluşturur.
            _memoryCache.GetOrCreate<string>("time", entry => DateTime.Now.ToString());
            #endregion

            _memoryCache.TryGetValue("time", out string cachedTime);
            _memoryCache.TryGetValue("callback", out string callback);

            ViewBag.Time = cachedTime;
            ViewBag.Callback = callback;
            ViewBag.Product = _memoryCache.Get<Product>("product:1");

            return View();
        }
    }
}
