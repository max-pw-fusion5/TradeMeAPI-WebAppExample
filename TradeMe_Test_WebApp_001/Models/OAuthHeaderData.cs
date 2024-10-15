using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace TradeMe_Test_WebApp_001.Models
{
    public class OAuthHeaderData
    {
        private static HttpContext _httpContext => new HttpContextAccessor().HttpContext;

        //your credentials for the consumer you are using in sandbox
        //DO NOT WRITE BY HAND SECRETS DIRECTLY IN HERE: only do so in a dev environment (and EVEN THEN, DON'T)
        //Secrets should be fetched from a secure source, such as an Azure Key Vault
        //create your own application here https://www.tmsandbox.co.nz/MyTradeMe/Api/RegisterNewApplication.aspx
        //your consumer key and secret should then be filled out in the vars below (but not by hand, set them from somewhere else in the application, or don't store them here at all) 
        public static string ConsumerKey { get; set; } = "996744A5B949CDB35DC780C608FA20B0";
        public static string ConsumerSecret { get; set; } = "769E31A1C7A715A2494C7589FED92DEB";
        //this callback must have the same domain as the one you registered with us when you created your application
        public const string OAuthVersion = "1.0";
        public const string OAuthNonce = "7O3kEe";
        public const string SignatureMethod = "PLAINTEXT";
        public static readonly string Callback = HttpUtility.UrlEncode("https://" + _httpContext.Request.Host + "/oauth/callback");

    }
}
