using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sicolnet.Utils
{
    public class SmsSender
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
        private static void SendSMS(List<string> numeros, string text)
        {
            string result = "";
            foreach (string n in numeros)
                result += n + ",";
            result = result.Substring(0, result.Length - 1);
            SendSMS(result, text);
        }

        public static async Task<string> Send(string numeros, string text)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "account", Account },
                    { "apiKey", ApiKey },
                    { "sms", text },
                    { "token", Token },
                    { "toNumber", numeros }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("https://api101.hablame.co/api/sms/v2.1/send/", content);

                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
        }

        public  static async void SendSMS(string numeros, string text)
        {
            var values = new Dictionary<string, string>
                {
                    { "account", Account },
                    { "apiKey", ApiKey },
                    { "sms", text },
                    { "token", Token },
                    { "toNumber", numeros }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api101.hablame.co/api/sms/v2.1/send/", content);

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
