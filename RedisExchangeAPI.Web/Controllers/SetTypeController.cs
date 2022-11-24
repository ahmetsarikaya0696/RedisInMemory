using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : BaseController
    {
        private readonly string key = "setTypeNames";
        public SetTypeController(IDatabase redisDb) : base(redisDb)
        {
        }
        public IActionResult Index()
        {
            HashSet<string> nameList = new();

            if (_redisDb.KeyExists(key))
            {
                nameList = _redisDb.SetMembers(key).Select(rv => rv.ToString()).ToHashSet();
            }

            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            if (!_redisDb.KeyExists(key)) _redisDb.KeyExpire(key, DateTime.Now.AddMinutes(5));

            _redisDb.SetAdd(key, name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(string name)
        {
            _redisDb.SetRemove(key, name);
            return RedirectToAction(nameof(Index));
        }
    }
}
