using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace SPDC.Service
{
    public class ApiHelper
    {
        private readonly IRestClient _client;
        private static ApiHelper _instance;

        private ApiHelper()
        {
            var baseUrl = WebConfigurationManager.AppSettings["API_BASE_URL"];

            if (string.IsNullOrEmpty(baseUrl))
            {
                _client = new RestClient();
            }
            else
            {
                _client = new RestClient(baseUrl);
            }
        }

        public static ApiHelper Instance()
        {
            if (_instance == null)
            {
                _instance = new ApiHelper();
            }

            return _instance;
        }

        public async Task<TResponse> Post<TRequest, TResponse>(string resource, TRequest model)
        {
            var req = new RestRequest(resource, Method.POST, DataFormat.Json);
            req.AddHeader("x-api-key", "Test1");
            req.AddJsonBody(model);

            var res = await _client.ExecuteAsync(req);

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<TResponse>(res.Content);
            }

            return Activator.CreateInstance<TResponse>();
        }

        public async Task<TResponse> Put<TRequest, TResponse>(string resource, TRequest model)
        {
            var req = new RestRequest(resource, Method.PUT, DataFormat.Json);
            req.AddHeader("x-api-key", "Test1");
            req.AddJsonBody(model);

            var res = await _client.ExecuteAsync(req);

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<TResponse>(res.Content);
            }

            return Activator.CreateInstance<TResponse>();
        }

        public async Task<TResponse> Get<TResponse>(string resource, List<KeyValuePair<string, string>> param)
        {
            var req = new RestRequest(resource, Method.GET, DataFormat.Json);
            req.AddHeader("x-api-key", "Test1");

            foreach (var item in param)
            {
                req.AddParameter(item.Key, item.Value);
            }

            var result = await _client.GetAsync<TResponse>(req);

            return result;
        }
    }
}
