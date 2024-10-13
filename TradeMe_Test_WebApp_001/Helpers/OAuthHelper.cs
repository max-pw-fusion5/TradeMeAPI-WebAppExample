using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Web;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
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

        public static readonly string RequestTokenKeyName = "RequestToken";
        public static readonly string RequestTokenSecretKeyName = "RequestTokenSecret";
        public static readonly string AccessTokenKeyName = "AccessToken";
        public static readonly string AccessTokenSecretKeyName = "AccessTokenSecret";

        private static SecretClientOptions options = new SecretClientOptions()
        {
            Retry =
            {
                Delay= TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };

        private static SecretClient client = new SecretClient(new Uri("https://tmcreds-maxpw425.vault.azure.net/"), new DefaultAzureCredential(), options);

        //retrieve the time in seconds since epoch
        private static string GetOauthTimestamp()
        {
            return Convert.ToString((DateTime.Now - new DateTime(1970, 01, 01)).TotalSeconds, CultureInfo.InvariantCulture);
        }

        public static string GetBaseOAuthHeader()
        {
            // "&" becomes %26 when Url encoded
            var signature = HttpUtility.UrlEncode(OAuthHelper.getSecret("ConsumerSecret") + "&");
            return string.Format(
                "OAuth oauth_callback={0}, oauth_consumer_key={1}, oauth_version={2}, oauth_timestamp={3}, oauth_nonce={4}, oauth_signature_method={5}, oauth_signature={6}",
                OAuthHeaderData.Callback, OAuthHelper.getSecret("ConsumerKey"), OAuthHeaderData.OAuthVersion, GetOauthTimestamp(), OAuthHeaderData.OAuthNonce, OAuthHeaderData.SignatureMethod, signature);
        }

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
                    client.SetSecret(secretName, secretValue);
                    return true;
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
                    string secretContent = string.Empty;
                    var secret = client.GetSecret(secretName).Value;
                    secretContent = secret.Value;
                    return secretContent;
            }
        }

    }
}
