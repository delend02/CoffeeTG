using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeTG.Telegram.Request
{
    internal class PostRequest
    {
        public Dictionary<string,string> Body { get; set; }

        public string Path { get; set; }

        readonly HttpClient _httpClient;
       
        public PostRequest(Dictionary<string, string> body, string path, HttpClient httpClient)
        {
            Body = body;
            Path = path;
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Operation()
        {
            try
            {
                var data = new FormUrlEncodedContent(Body);

                var response = await _httpClient.PostAsync(Path, data);

                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
