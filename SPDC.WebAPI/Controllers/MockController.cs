using SPDC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SPDC.WebAPI.Controllers
{
    [RoutePrefix("api/Mock")]
    public class MockController : ApiControllerBase
    {
        private readonly IAutoEmailService _autoMailService;

        public MockController(IAutoEmailService autoEmailService)
        {
            _autoMailService = autoEmailService;
        }

        [HttpGet]
        [Route("SendEmail")]
        // GET api/<controller>
        public IEnumerable<string> SendEmail()
        {
            _autoMailService.ClassCommencementReminder();

            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}