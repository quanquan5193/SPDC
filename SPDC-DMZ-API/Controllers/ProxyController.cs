using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Http = System.Net.WebRequestMethods.Http;

namespace SPDC_DMZ_API.Controllers
{
    [RoutePrefix("api/Proxy")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProxyController : ApiController
    {
        [AcceptVerbs(Http.Get, Http.Head, Http.MkCol, Http.Post, Http.Put)]
        public async Task<HttpResponseMessage> Proxy(string url)
        {
            using (HttpClient http = new HttpClient())
            {
                if (this.Request.Method == HttpMethod.Get)
                {
                    this.Request.Content = null;
                }

                HttpRequestMessage httpRequest = new HttpRequestMessage(this.Request.Method, new Uri($"{ConfigurationManager.AppSettings["RemoteWebSite"].ToString()}{url}"));
                http.DefaultRequestHeaders.Authorization = this.Request.Headers.Authorization;
                httpRequest.Headers.Add("Accept-Language", this.Request.Headers.AcceptLanguage.ToString());
                httpRequest.Content = this.Request.Content;

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                return await http.SendAsync(httpRequest);
            }
        }
    }
}