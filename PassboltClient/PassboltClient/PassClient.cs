using Newtonsoft.Json;
using Passbolt.Models;
using System.Text;

namespace Passbolt
{
    public class PassClient
    {
        public async Task Login(string baseUrl, LoginRequest loginRequest)
        {
            HttpClient client = new HttpClient();
            var url = $"{baseUrl}/auth/jwt/login.json";
            var body = JsonConvert.SerializeObject(loginRequest);

            var content = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                

                string responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseData);
            }
            catch (HttpRequestException e)
            {
                
                Console.WriteLine("Request error: " + e.Message);
            }
        }

        public async Task<ServerKeyResponse> GetServerPublicKey(string baseUrl)
        {
            HttpClient client = new HttpClient();
            var url = $"{baseUrl}/auth/verify.json";
            var httpResponse = await client.GetAsync(url);            
            var res = await httpResponse.Content.ReadAsStringAsync();
            ServerKeyResponse serverKey = JsonConvert.DeserializeObject<ServerKeyResponse>(res);
            return serverKey;
        }
    }
}
