using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StockApp.Models;
using StockApp.Helpers;
using StockApp.Repositories;
namespace StockApp.Controllers
{
    public class DocumentController : ApiController
    {
        IDBRepository dbRepository;
        // GET api/document

        public DocumentController()
        {
            dbRepository = new DBRepository();
        }
        public IEnumerable<Document> Get()
        {
            return dbRepository.GetLastDocuments();
        }

        // GET api/document/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
