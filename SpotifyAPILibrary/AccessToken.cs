using SpotifyAPILibrary.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SpotifyAPILibrary
{
    /// <summary>
    /// 
    /// </summary>
    public static class AccessToken
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scopes"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="showDialog"></param>
        /// <returns></returns>
        public static UserToken RequestUserToken(string scopes, string clientId = "bb457095cedf43b4ad05b1b1956f50b8", string clientSecret = "ab285ecc47ba4e008a2a72e797a3c567", bool showDialog = false)
        {
            UriBuilder uriBuilder = new UriBuilder("https://accounts.spotify.com/authorize");
            uriBuilder.Query += $"client_id={clientId}";
            uriBuilder.Query += $"&response_type=code";
            uriBuilder.Query += $"&redirect_uri=http://localhost:8000/auth";
            uriBuilder.Query += $"&state=state";
            uriBuilder.Query += $"&scope={scopes}";
            uriBuilder.Query += $"&show_dialog={showDialog}";

            string url = uriBuilder.Uri.ToString().Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", "/c start " + url));

            var authcode = Server.HandleIncomingConnections().Result;

#if DEBUG
            Console.WriteLine("Authorization code: " + authcode);
            Console.WriteLine();
#endif

            return RequestUserToken(authcode, clientId, clientSecret);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        public static UserToken RequestUserToken(string code, string clientId, string clientSecret)
        {
            var requestBody = new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", "http://localhost:8000/auth" }
            };

            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://accounts.spotify.com/api/token"),
                Method = HttpMethod.Post,
                Content = new FormUrlEncodedContent(requestBody),
            };

            byte[] plainBytes = Encoding.UTF8.GetBytes(clientId + ":" + clientSecret);

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(plainBytes));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            HttpClient client = new HttpClient();

            var response = client.SendAsync(request).Result;

            client.Dispose();

            if (response.IsSuccessStatusCode)
            {
#if DEBUG
                var debugJson = JsonSerializer.Deserialize<UserToken>(response.Content.ReadAsStream());
                Console.WriteLine();
                return debugJson;
#endif
                return JsonSerializer.Deserialize<UserToken>(response.Content.ReadAsStream());
            }

            return null;
        }
    }
}
