using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IDatabase _redisDb;

        public BaseController(IDatabase redisDb)
        {
            _redisDb = redisDb;
        }
    }
}
