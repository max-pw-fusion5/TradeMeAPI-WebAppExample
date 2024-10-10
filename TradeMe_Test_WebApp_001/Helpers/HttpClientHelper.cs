namespace TradeMe_Test_WebApp_001.Helpers
{
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Http.Headers;

    public class HttpClientHelper
    {

        public static HttpClient sharedClient = new()
        {
        };

        public static string MakeHttpRequest(Uri url, string authHeader, HttpMethod method)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = url
            };

            request.Headers.Add("Authorization", authHeader);


            var responseText = "Unknown response text";
            try
            {
                var task = sharedClient.SendAsync(request)
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;

                    var stream = response.Content.ReadAsStream();
                    if (stream != null)
                        using (var reader = new StreamReader(stream))
                        {
                            responseText = reader.ReadToEnd();
                            response.Dispose();
                            reader.Dispose();
                        }
                });
                task.Wait();
            }
            catch (WebException e)
            {
                var resp = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();

                //catch any errors and display in the debug window
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Debug.WriteLine("Status Code string : {0}", ((HttpWebResponse)e.Response).StatusCode);
                    Debug.WriteLine("Status Code number : {0:D}", ((HttpWebResponse)e.Response).StatusCode);
                    Debug.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                    Debug.WriteLine("Error Message : {0}", resp);
                    throw new WebException(resp, e);
                }
            }

            return responseText;
        }

    }
}
