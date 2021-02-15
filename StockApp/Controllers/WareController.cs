using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StockApp.Helpers;
using StockApp.Models;
using StockApp.Repositories;

namespace StockApp.Controllers
{
    public class WareController : ApiController
    {
        IDBRepository dbRepository;

        public WareController()
        {
            dbRepository = new DBRepository();
        }

        // GET api/ware/5
        public IEnumerable<WareInfo> Get(string id)
        {
            return dbRepository.GetWareInfo(id);
        }

        // POST api/ware
        public void Post([FromBody]string value)
        {
        }

        // PUT api/ware/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/ware/5
        public void Delete(int id)
        {
        }
    }
}
