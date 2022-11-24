using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : BaseController
    {
        private readonly string key = "names";
        public ListTypeController(IDatabase redisDb) : base(redisDb)
        {
        }

        public IActionResult Index()
        {
            List<string> nameList = new();

            if (_redisDb.KeyExists(key))
            {
                nameList = _redisDb.ListRange(key).Select(rv => rv.ToString()).ToList();
            }

            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            _redisDb.ListRightPush(key, name);
            //_redisDb.ListLeftPush(key, name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(string name)
        {
            _redisDb.ListRemove(key, name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFirst()
        {
            _redisDb.ListLeftPop(key);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveLast()
        {
            _redisDb.ListRightPop(key);
            return RedirectToAction(nameof(Index));
        }
    }
}
