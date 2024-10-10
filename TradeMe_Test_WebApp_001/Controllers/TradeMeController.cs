using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradeMe_Test_WebApp_001.Helpers;

namespace TradeMe_Test_WebApp_001.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TradeMeController : ControllerBase
    {
        private const string MyTradeMeSummaryUrl = "https://api.tmsandbox.co.nz/v1/MyTradeMe/Summary.json";

        [Route("/trade-me/summary")]
        [HttpGet]
        public IActionResult GetRequestToken()
        {
            var authHeader = string.Format("{0}{1}, oauth_token={2}", OAuthHelper.GetBaseOAuthHeader(), OAuthHelper.getSecret("temp_AccessTokenSecretKeyName"), OAuthHelper.getSecret("temp_AccessTokenKeyName"));
            var response = HttpClientHelper.MakeHttpRequest(new Uri(MyTradeMeSummaryUrl), authHeader, HttpMethod.Get);

            return Ok(response);
        }
    }
}
