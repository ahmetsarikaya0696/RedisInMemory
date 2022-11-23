using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly IDatabase _redisDb;
        private readonly string key = "hashTypeNames";

        public HashTypeController(IDatabase redisDb)
        {
            _redisDb = redisDb;
        }

        public IActionResult Index()
        {
            Dictionary<string, string> keyValuePairs = new();

            if (_redisDb.KeyExists(key))
            {
                _redisDb.HashGetAll(key).ToList().ForEach(x => keyValuePairs.Add(x.Name, x.Value));
            }

            return View(keyValuePairs);
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            _redisDb.HashSet(key, name, value);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(string name)
        {
            _redisDb.HashDelete(key, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
