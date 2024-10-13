using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using System.Text.RegularExpressions;
using TradeMe_Test_WebApp_001.Helpers;

namespace TradeMe_Test_WebApp_001.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private const string TradeMeOAuthApiUrl = "https://secure.tmsandbox.co.nz/Oauth/";
        private readonly Regex _oauthRequestTokenRegex = new Regex("oauth_token=(\\w+)&oauth_token_secret=(\\w+)&oauth_callback_confirmed=(\\w+)");
        private readonly Regex _oauthAccessTokenRegex = new Regex("oauth_token=(\\w+)&oauth_token_secret=(\\w+)");

        private const string RequestTokenKeyName = "RequestToken";
        private const string RequestTokenSecretKeyName = "RequestTokenSecret";
        private const string AccessTokenKeyName = "AccessToken";
        private const string AccessTokenSecretKeyName = "AccessTokenSecret";

        // GET: /OAuth/GetRequestToken
        //this is the first step. Before the page loads we make a request to TradeMe to get some request tokens, these are temporary.
        //You will use the request token to send to Trade Me when the user is asked to login to their Trade Me account, this is so
        //that you know who has approved access to your application
        [Route("/oauth/request-token")]
        [HttpGet]
        public IActionResult GetRequestToken()
        {
            //set up the URL we will be using to get the request tokens, in this case we want to be able to read and write to the users account
            var url = string.Format("{0}RequestToken?scope=MyTradeMeRead,MyTradeMeWrite", TradeMeOAuthApiUrl);

            var authHeader = OAuthHelper.GetBaseOAuthHeader();

            //make the request to get the request tokens and store the result in responseText
            //responseText looks like this:
            //oauth_token=XXXXXXXX&oauth_token_secret=XXXXXXXXXX&oauth_callback_confirmed=true
            string responseText = "";
            try
            {
                responseText = HttpClientHelper.MakeHttpRequest(new Uri(url), authHeader, HttpMethod.Post);
            }
            catch (WebException e)
            {
                // TODO: Error handling here

                return BadRequest("Exception with request for temporary tokens");
            }

            //get the values out using regex
            var matches = _oauthRequestTokenRegex.Match(responseText);

            if(matches.Groups.Count <= 1)
            {
                return BadRequest("Error: Token could not be retrieved \n ----- \nReason: " + responseText);
            }

            //TODO: store temporary token + token secret into azure keyvault

            OAuthHelper.setSecret(RequestTokenKeyName, matches.Groups[1].ToString());
            OAuthHelper.setSecret(RequestTokenSecretKeyName, matches.Groups[2].ToString());

            //TODO: Generate (and send/redirect to/open browser with?) URL for user to log in and give permission to application 

            string authorizeUrl = string.Format("https://secure.tmsandbox.co.nz/Oauth/Authorize?oauth_token={0}", OAuthHelper.getSecret(RequestTokenKeyName));

            return Redirect(authorizeUrl);
        }

        // GET: /oauth/callback
        //this is the callback we set up upon registration of our application and was used in step one as the oauth_callback,
        //this URL is hit when Trade Me redirects the user back to this application after they have approved access or logged in
        //the query string will contain the oauth token (same token as the one above) and a verifier to say the user has given access to their account
        [Route("/oauth/callback")]
        [HttpGet]
        public IActionResult Callback(string oauth_token, string oauth_verifier)
        {
            //this just makes sure if we hit this url without the token, verifier or having a current request token we return an error
            if (string.IsNullOrWhiteSpace(oauth_token) || string.IsNullOrWhiteSpace(oauth_verifier) || OAuthHelper.getSecret(RequestTokenKeyName) == null || OAuthHelper.getSecret(RequestTokenSecretKeyName) == null)
            {
                return BadRequest("ERROR: token or verifier error");
            }
            var url = string.Format("{0}AccessToken", TradeMeOAuthApiUrl);

            //still use the base header, but add the token secret to the existing signature "<consumer_key>&" + "<oauth_token_secret>"
            //the Session["RequestTokenSecret"] is the oauth_token_secret and the base header already ends in "&"
            var authHeader = string.Format(
            "{0}{1}, oauth_verifier={2}, oauth_token={3}",
            OAuthHelper.GetBaseOAuthHeader(),
            OAuthHelper.getSecret(RequestTokenSecretKeyName), 
            oauth_verifier, 
            OAuthHelper.getSecret(RequestTokenKeyName));

            //make the last request to get our ACCESS tokens, these are permenant and can be used to authorize as the user on the API
            var responseText = HttpClientHelper.MakeHttpRequest( new Uri(url), authHeader, HttpMethod.Post);

            var matches = _oauthAccessTokenRegex.Match(responseText);

            //TODO: save access token and token secret
            OAuthHelper.setSecret(AccessTokenKeyName, matches.Groups[1].ToString());
            OAuthHelper.setSecret(AccessTokenSecretKeyName, matches.Groups[2].ToString());

            return Ok("Access Tokens Acquired :D");
        }
    }
}
