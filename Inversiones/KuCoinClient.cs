using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace Inversiones
{

    public class KuCoinClient
    {
        private string _apiSecret = "789b9463-62cd-434d-901c-a71979d4f37b";
        private string _apiKey = "64895157b6d6670001d3f68f";
        private string _passPhrase = "Zixelowe1";
        private string _baseUri = "https://api.kucoin.com";

        public async Task<decimal> GetAccountBalanceAsync()
        {
            decimal totalBalance = 0;

            // Create the request message
            var endpoint = "/api/v1/accounts";
            var requestMessage = CreateHttpRequest(HttpMethod.Get, endpoint);

            // Send the request
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(requestMessage);
                var responseString = await response.Content.ReadAsStringAsync();

                var kuCoinResponse = JsonConvert.DeserializeObject<KuCoinResponse>(responseString);

                // Filter balances greater or equal than 1 and calculate the sum
                
                foreach (var account in kuCoinResponse.Data)
                {
                    if (decimal.TryParse(account.Balance, out decimal balance))
                    {
                       // result += $"Currency: {account.Currency}, Balance: {account.Balance}, ";
                        totalBalance += balance;
                    }
                }

                // Append total balance to the result string
                
            }

            return totalBalance;
        }

        private HttpRequestMessage CreateHttpRequest(HttpMethod method, string endpoint)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var strToSign = now + "GET" + endpoint;

            var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_apiSecret));
            var signature = Convert.ToBase64String(hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(strToSign)));

            var passphrase = Convert.ToBase64String(hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(_passPhrase)));

            var requestMessage = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(_baseUri + endpoint)
            };

            requestMessage.Headers.Add("KC-API-SIGN", signature);
            requestMessage.Headers.Add("KC-API-TIMESTAMP", now.ToString());
            requestMessage.Headers.Add("KC-API-KEY", _apiKey);
            requestMessage.Headers.Add("KC-API-PASSPHRASE", passphrase);
            requestMessage.Headers.Add("KC-API-KEY-VERSION", "2");

            return requestMessage;
        }

        public async Task<bool> verificarIP()
        {
            string expectedIP = "152.207.224.86"; // IP esperada
            string response;
            using (HttpClient client = new HttpClient())
            {
                response = await client.GetStringAsync("https://api.ipify.org");
               
            }

            return response == expectedIP ? true : false;
          
        }


        public async Task<bool> CreateMarginMarketOrderAsync(string symbol, string side, decimal quantity, string leverage)
        {
            var endpoint = "/api/v1/margin/order";

            var body = new
            {
                clientOid = Guid.NewGuid().ToString("N"),
                side, // buy or sell
                symbol, // currency pair, eg. BTC-USDT
                type = "market",
                size = quantity.ToString(),
                leverage = leverage,
                autoBorrow = true, // enable auto borrow
            };

            string jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Create signature
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string strToSign = $"{now}POST{endpoint}{jsonBody}";
            var hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_apiSecret));
            var signature = Convert.ToBase64String(hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(strToSign)));
            var passphrase = Convert.ToBase64String(hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(_passPhrase)));

            // Create request
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_baseUri + endpoint),
                Content = content
            };

            // Add headers
            requestMessage.Headers.Add("KC-API-SIGN", signature);
            requestMessage.Headers.Add("KC-API-TIMESTAMP", now.ToString());
            requestMessage.Headers.Add("KC-API-KEY", _apiKey);
            requestMessage.Headers.Add("KC-API-PASSPHRASE", passphrase);
            requestMessage.Headers.Add("KC-API-KEY-VERSION", "2");

            // Send the request
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(requestMessage);

                // Optional: check the response status code and handle errors
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    // handle error
                    return false;
                }
            }

            // If everything is fine, return true
            return true;
        }




        public class KuCoinResponse
        {
            public List<KuCoinAccount> Data { get; set; }
        }

        public class KuCoinAccount
        {
            public string Currency { get; set; }
            public string Balance { get; set; }
        }
    }


}
