using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BoatToursManager.Controllers
{
    public class ValuesApiController : ApiController
    {
        static List<string> allValues = new List<string> { "value1", "value2" };

        // GET /api/values
        public IEnumerable<string> Get()
        {
            return allValues;
        }

        // GET /api/values/5
        public string Get(int id)
        {
            if (id < allValues.Count)
            {
                return allValues[id];
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // POST /api/values
        public void Post(string value)
        {
            
        }

        // PUT /api/values/5
        public void Put(int id, string value)
        {
            if (id < allValues.Count)
            {
                allValues[id] = value;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // DELETE /api/values/5
        public void Delete(int id)
        {
            if (id < allValues.Count)
            {
                allValues.RemoveAt(id);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
