using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sicolnet.Utils
{

    public class UrlShort
    {
        public string token { get; set; }
        public string short_url { get; set; }
        public string url { get; set; }

    }

    public struct Trace { 
        public string name { get; set; }
        public string version { get; set; } 
    }

    public class UrlShortError
    {
        public string code { get; set; }
        public string details { get; set; }
        public List<Trace> trace { get; set; }
    }

    public class UrlShorterResponse
    {
        public UrlShort  data { get; set; }
        public UrlShortError error { get; set; }
    }

    public class UrlShorter
    {

        private static string Account { get; set; }
        private static string ApiKey { get; set; }
        private static string Token { get; set; }

        public static void SetConfig(string _account, string _apikey, string _token)
        {
            Account = _account;
            ApiKey = _apikey;
            Token = _token;

            
        }

        public static async Task<UrlShorterResponse> ShortUrl(string url)
        {

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("account", Account);
            client.DefaultRequestHeaders.Add("apikey", ApiKey);
            client.DefaultRequestHeaders.Add("token", Token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            string jsondata = "{ \"url\": \"" +url + "\" }";

            StringContent strc = new StringContent(jsondata,
                                                    Encoding.UTF8,
                                                    "application/json");
            var response = await client.PostAsync("https://api103.hablame.co/api/url-shortener/v1/token", strc);

            var responseString = await response.Content.ReadAsStringAsync();
            UrlShorterResponse rr = JsonConvert.DeserializeObject<UrlShorterResponse>(responseString);
            client.Dispose();
            return rr;
        }
    }
}
