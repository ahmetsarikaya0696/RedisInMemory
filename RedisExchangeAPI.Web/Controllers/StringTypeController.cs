using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : BaseController
    {
        private readonly string key = "stringTypeNames";
        private readonly string count= "stringTypeVisitorCount";

        public StringTypeController(IDatabase redisDb) : base(redisDb)
        {
        }

        public IActionResult Index()
        {
            _redisDb.StringSet(key, "ahmet sarikaya");
            _redisDb.StringSet(count, 100);
            return View();
        }

        public IActionResult Show()
        {
            //_redisDb.StringIncrement(count, 1);
            _redisDb.StringDecrement(count, 1);

            //var cachedName = _redisDb.StringGet(key);
            //var cachedName = _redisDb.StringGetRange(key, 0, 3);
            var cachedName = _redisDb.StringLength(key);

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
