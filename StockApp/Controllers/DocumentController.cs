using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StockApp.Models;
using StockApp.Helpers;

namespace StockApp.Controllers
{
    public class DocumentController : ApiController
    {
        // GET api/document
        public IEnumerable<Document> Get()
        {
            return DBConnection.GetLastDocuments();
        }

        // GET api/document/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
