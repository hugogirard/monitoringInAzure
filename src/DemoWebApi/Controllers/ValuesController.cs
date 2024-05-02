using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Serilog;

namespace DemoWebApi.Controllers
{
    public class ValuesController : ApiController
    {
        static List<string> values = new List<string>() { "value1", "value2" };

        // GET api/values
        public IEnumerable<string> Get()
        {
            Log.Information("GET api/values called");
            return values;
        }

        // GET api/values/5
        public string Get(int id)
        {
            if (id < 0 || id >= values.Count)
            {
                Log.Warning("GET api/values/{Id} not found", id);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            Log.Information("GET api/values/{Id} called", id);
            return values[id];
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
            values.Add(value);
            Log.Information("POST api/values called with value: {Value}", value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
            if (id < 0 || id >= values.Count)
            {
                Log.Warning("PUT api/values/{Id} not found", id);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            values[id] = value;
            Log.Information("PUT api/values/{Id} called with value: {Value}", id, value);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            if (id < 0 || id >= values.Count)
            {
                Log.Warning("DELETE api/values/{Id} not found", id);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            values.RemoveAt(id);
            Log.Information("DELETE api/values/{Id} called", id);
        }
    }
}
