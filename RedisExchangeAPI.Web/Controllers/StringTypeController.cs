using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly IDatabase _redisDb;

        public StringTypeController(IDatabase redisDb)
        {
            _redisDb = redisDb;
        }

        public IActionResult Index()
        {
            _redisDb.StringSet("name", "ahmet sarikaya");
            _redisDb.StringSet("visitorCount", 100);
            return View();
        }

        public IActionResult Show()
        {
            //_redisDb.StringIncrement("visitorCount", 1);
            _redisDb.StringDecrement("visitorCount", 1);

            //var cachedName = _redisDb.StringGet("name");
            //var cachedName = _redisDb.StringGetRange("name", 0, 3);
            var cachedName = _redisDb.StringLength("name");

            ViewBag.cachedName = cachedName.ToString();

            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/togg.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _redisDb.StringSet("img:1", imageByte);
            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _redisDb.StringGet("img:1");
            return File(imageByte, "image/jpg");
        }
    }
}
