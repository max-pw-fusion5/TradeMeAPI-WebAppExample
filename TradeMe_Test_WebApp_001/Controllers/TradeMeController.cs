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
            var authHeader = string.Format("{0}{1}, oauth_token={2}", OAuthHelper.GetBaseOAuthHeader(), OAuthHelper.getSecret(OAuthHelper.AccessTokenSecretKeyName), OAuthHelper.getSecret(OAuthHelper.AccessTokenKeyName));
            var response = HttpClientHelper.MakeHttpRequest(new Uri(MyTradeMeSummaryUrl), authHeader, HttpMethod.Get);

            return Ok(response);
        }

        [Route("/trade-me/sold-items/{filter}.{file_format}")]
        [HttpGet]
        public IActionResult GetSoldItems(string filter, string file_format)
        {
            var authHeader = string.Format("{0}{1}, oauth_token={2}", OAuthHelper.GetBaseOAuthHeader(), OAuthHelper.getSecret(OAuthHelper.AccessTokenSecretKeyName), OAuthHelper.getSecret(OAuthHelper.AccessTokenKeyName));

            string url = string.Format("{0}/{1}.{2}", MyTradeMeSoldItemsUrl_Base, filter, file_format);
            var response = HttpClientHelper.MakeHttpRequest(new Uri(MyTradeMeSoldItemsUrl_Base), authHeader, HttpMethod.Get);

            return Ok(response);
        }
    }
}
