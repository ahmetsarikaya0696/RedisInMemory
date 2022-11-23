using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly IDatabase _redisDb;
        private readonly string key = "sortedSetTypeNames";

        public SortedSetTypeController(IDatabase redisDb)
        {
            _redisDb = redisDb;
        }

        public IActionResult Index()
        {
            HashSet<string> nameList = new();

            if (_redisDb.KeyExists(key))
            {
                //nameList = _redisDb.SortedSetScan(key).Select(x => x.ToString()).ToHashSet();
                nameList = _redisDb.SortedSetRangeByRank(key, order: Order.Descending).Select(x => x.ToString()).ToHashSet();
            }

            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name, double score)
        {
            _redisDb.SortedSetAdd(key, name, score);
            _redisDb.KeyExpire(key, DateTime.Now.AddMinutes(5));

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(string name)
        {
            _redisDb.SortedSetRemove(key, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
