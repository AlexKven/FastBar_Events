using FastBar_Events.Models;
using ModernHttpClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FastBar_Events
{
    public static class APIManager
    {
        public static string CipherToken(string token)
        {
            if (token == null)
                return null;
            //Ciphers a string (presumably the access token) with the cipher in Keys.cs
            //using a simple XOR cipher. Since it's an XOR cipher, deciphering is done by
            //simply running the ciphered string through this function again.
            int cipherLength = Keys.TokenCipher.Length;
            char[] result = new char[token.Length];
            for (int i = 0; i < token.Length; i++)
            {
                //Result is the token char XOR'd with the cipher
                result[i] = (char)((byte)token[i] ^ (byte)Keys.TokenCipher[i % cipherLength]);
            }
            return new string(result);
        }

        public static async Task<string> GetToken(string username, string password)
        {
            //Method for logging in and obtaining an access token.
            //var client = new HttpClient();
            HttpResponseMessage resp;
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                resp = await client.PostAsync("https://fastbar-test.azurewebsites.net/token", new StringContent($"grant_type=password&username={username}&password={password}", Encoding.UTF8, "application/x-www-form-urlencoded"));
            }
            var respObj = JObject.Parse(await resp.Content.ReadAsStringAsync());
            JToken token;
            if (respObj.TryGetValue("access_token", out token))
            {
                //The token was returned successfully
                return token.ToString();
            }
            else
            {
                //No token was returned for some reason. Probably bad login information.
                return null;
            }
        }
        
        public static async Task<IEnumerable<Event>> GetEvents(string token)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://fastbar-test.azurewebsites.net/api/Events?userTypeFilter=Operating");
            System.Diagnostics.Debug.WriteLine(token);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Method = HttpMethod.Get;
            HttpResponseMessage resp;
            using (var client = new HttpClient())
            {
                resp = await client.SendAsync(request);
            }
            List<Event> results = new List<Event>();
            string str = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
            {
                //Denote failure by returning null, which is different from an empty list,
                //which the function would return in the case of there being no events.
                return null;
            }

            var arrayObject = JArray.Parse(await resp.Content.ReadAsStringAsync());
            foreach (var item in arrayObject.Children())
            {
                results.Add(item.ToObject<Event>());
            }

            return results;
        }
    }
}
