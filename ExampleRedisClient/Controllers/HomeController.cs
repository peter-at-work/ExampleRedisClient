using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExampleRedisClient.Models;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ExampleRedisClient.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Test(DataModel model, string op)
        {
            if (model.Key == null)
            {
                return View("Data");
            }

            switch (op)
            {
                case "Get":
                    return await GetData(model);
                case "Put":
                    return await PutData(model);
                default:
                    return View("Data");
            }
        }

        public async Task<ActionResult> GetData(DataModel model)
        {
            var redisConnection = SingletonServices.ServiceProvider.GetService<IConnectionMultiplexer>();
            var db = redisConnection.GetDatabase();

            var key = model.Key;

            var stopwatch = Stopwatch.StartNew();
            var val = await db.StringGetAsync(key);
            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours,
                ts.Minutes,
                ts.Seconds,
                ts.Milliseconds / 10);

            return View("Data", new DataModel { Key = key, Value = val, ElapsedTime = elapsedTime });
        }

        public async Task<ActionResult> PutData(DataModel model)
        {
            var redisConnection = SingletonServices.ServiceProvider.GetService<IConnectionMultiplexer>();
            var db = redisConnection.GetDatabase();

            var key = model.Key;
            var val = model.Value;

            var stopwatch = Stopwatch.StartNew();
            await db.StringSetAsync(key, val);
            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours,
                ts.Minutes,
                ts.Seconds,
                ts.Milliseconds / 10);

            return View("Data", new DataModel { Key = key, Value = val, ElapsedTime = elapsedTime });
        }

        public ActionResult Index()
        {
            return View("Data");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
