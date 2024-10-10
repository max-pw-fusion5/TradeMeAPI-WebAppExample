using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Web;
using TradeMe_Test_WebApp_001.Models;

namespace TradeMe_Test_WebApp_001.Helpers
{
    public static class OAuthHelper
    {

        //TODO: delete these variables and implement
        //Azure Key Vault secret fetching
        private static string temp_RequestTokenKeyName = string.Empty;
        private static string temp_RequestTokenSecretKeyName = string.Empty;

        private static string temp_AccessTokenKeyName = string.Empty;
        private static string temp_AccessTokenSecretKeyName = string.Empty;

        //retrieve the time in seconds since epoch
        private static string GetOauthTimestamp()
        {
            return Convert.ToString((DateTime.Now - new DateTime(1970, 01, 01)).TotalSeconds, CultureInfo.InvariantCulture);
        }

        public static string GetBaseOAuthHeader()
        {
            // "&" becomes %26 when Url encoded
            var signature = HttpUtility.UrlEncode(OAuthHeaderData.ConsumerSecret + "&");
            return string.Format(
                "OAuth oauth_callback={0}, oauth_consumer_key={1}, oauth_version={2}, oauth_timestamp={3}, oauth_nonce={4}, oauth_signature_method={5}, oauth_signature={6}",
                OAuthHeaderData.Callback, OAuthHeaderData.ConsumerKey, OAuthHeaderData.OAuthVersion, GetOauthTimestamp(), OAuthHeaderData.OAuthNonce, OAuthHeaderData.SignatureMethod, signature);
        }

        //public static string MakeHttpRequest(string method, Uri url, string header)
        //{

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = method;
        //    //we don't have a post body for any of the examples so must manually set the content length to 0
        //    //this is for example only, if you were going to be making post requests with a body then this won't work
        //    request.ContentLength = 0;

        //    request.Headers.Add("Authorization", header);
        //    var responseText = "Unknown response text";
        //    try
        //    {
        //        var response = (HttpWebResponse)request.GetResponse();
        //        var stream = response.GetResponseStream();
        //        if (stream != null)
        //            using (var reader = new StreamReader(stream))
        //            {
        //                responseText = reader.ReadToEnd();
        //                response.Dispose();
        //                reader.Dispose();
        //            }
        //    }
        //    catch (WebException e)
        //    {
        //        var resp = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();

        //        //catch any errors and display in the debug window
        //        if (e.Status == WebExceptionStatus.ProtocolError)
        //        {
        //            Debug.WriteLine("Status Code string : {0}", ((HttpWebResponse)e.Response).StatusCode);
        //            Debug.WriteLine("Status Code number : {0:D}", ((HttpWebResponse)e.Response).StatusCode);
        //            Debug.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
        //            Debug.WriteLine("Error Message : {0}", resp);
        //            throw new WebException(resp, e);
        //        }
        //    }
        //    return responseText;
        //}

        public static bool setSecret(string secretName, string secretValue)
        {
            switch (secretName)
            {
                case "temp_RequestTokenKeyName":
                    temp_RequestTokenKeyName = secretValue;
                    return true;
                case "temp_RequestTokenSecretKeyName":
                    temp_RequestTokenSecretKeyName = secretValue;
                    return true;
                case "temp_AccessTokenKeyName":
                    temp_AccessTokenKeyName = secretValue;
                    return true;
                case "temp_AccessTokenSecretKeyName":
                    temp_AccessTokenSecretKeyName = secretValue;
                    return true;
                default:
                    return false;
            }
        }

        public static string getSecret(string secretName)
        {
            switch (secretName)
            {
                case "temp_RequestTokenKeyName":
                    return temp_RequestTokenKeyName;
                case "temp_RequestTokenSecretKeyName":
                    return temp_RequestTokenSecretKeyName;
                case "temp_AccessTokenKeyName":
                    return temp_AccessTokenKeyName;
                case "temp_AccessTokenSecretKeyName":
                    return temp_AccessTokenSecretKeyName;
                default:
                    return string.Empty;
            }
        }

    }
}
