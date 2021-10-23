using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sicolnet.Utils
{
    public class UrlShorter
    {
        private static readonly HttpClient client = new HttpClient();

        private static string Account { get; set; }
        private static string ApiKey { get; set; }
        private static string Token { get; set; }

        public static void SetConfig(string _account, string _apikey, string _token)
        {
            Account = _account;
            ApiKey = _apikey;
            Token = _token;
        }

        public static async Task<string> ShortUrl(string url)
        {
           
            var content = new StringContent("https://api103.hablame.co/api/url-shortener/v1");

            client.DefaultRequestHeaders.Add("account", Account);
            client.DefaultRequestHeaders.Add("apikey", ApiKey);
            client.DefaultRequestHeaders.Add("token", Token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string jsondata = "{ \"url\": \"" +url + "\" }";

            StringContent strc = new StringContent(jsondata,
                                                    Encoding.UTF8,
                                                    "application/json");
            var response = await client.PostAsync("https://api103.hablame.co/api/url-shortener/v1", content);

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
