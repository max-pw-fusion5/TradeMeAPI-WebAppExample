using Microsoft.AspNetCore.Mvc;

namespace TradeMe_Test_WebApp_001.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : Controller
    {
        [Route("/hello-world")]
        [HttpGet]
        public string HelloWorld()
        {
            return "Hello World!";
        }
    }
}
