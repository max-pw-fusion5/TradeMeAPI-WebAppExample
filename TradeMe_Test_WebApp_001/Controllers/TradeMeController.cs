using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using TradeMe_Test_WebApp_001.Helpers;

namespace TradeMe_Test_WebApp_001.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TradeMeController : ControllerBase
    {
        private const string MyTradeMeSummaryUrl = "https://api.tmsandbox.co.nz/v1/MyTradeMe/Summary.json";
        private const string MyTradeMeSoldItemsUrl_Base = "https://api.tmsandbox.co.nz/v1/MyTradeMe/SoldItems";

        [Route("/trade-me/summary")]
        [HttpGet]
        public IActionResult GetSummary()
        {
            var authHeader = OAuthHelper.GetTokenedOAuthHeader();
            ObjectResult httpResponse = ResolveOAuth(authHeader);
            if (httpResponse.StatusCode < 400) {
                var response = HttpClientHelper.MakeHttpRequest(new Uri(MyTradeMeSummaryUrl), authHeader, HttpMethod.Get);
                httpResponse = Ok(response);
            }
            return httpResponse;
        }

        [Route("/trade-me/sold-items/{filter}.{file_format}")]
        [HttpGet]
        public IActionResult GetSoldItems(string filter, string file_format)
        {
            var authHeader = OAuthHelper.GetTokenedOAuthHeader();
            ObjectResult httpResponse = ResolveOAuth(authHeader);
            if (httpResponse.StatusCode < 400)
            {
                string url = string.Format("{0}/{1}.{2}", MyTradeMeSoldItemsUrl_Base, filter, file_format);
                var response = HttpClientHelper.MakeHttpRequest(new Uri(MyTradeMeSoldItemsUrl_Base), authHeader, HttpMethod.Get);

                httpResponse = Ok(response);
            }
            return httpResponse;
        }

        private ObjectResult ResolveOAuth(string authHeader)
        {
            if (authHeader == "-1")
            {
                return Unauthorized("Error: No OAuth credentials can be found");
            }

            return Ok(authHeader);
        }
    }
}
