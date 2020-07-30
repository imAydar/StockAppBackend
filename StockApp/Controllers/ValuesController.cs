using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using V83;
namespace StockApp.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values/5
        public Models.Document Get(int id)
        {
            return new Models.Document()
            {
                Date = DateTime.Now,
                Number = "somecode",
                Owner = "Baqchalarda"
            };
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
