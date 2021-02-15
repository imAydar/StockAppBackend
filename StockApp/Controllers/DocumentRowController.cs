using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StockApp.Models;
using StockApp.Helpers;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Web.SessionState;
using StockApp.Repositories;

namespace StockApp.Controllers
{
    public class DocumentRowController : ApiController
    {
        IDBRepository dbRepository;

        public DocumentRowController()
        {
            dbRepository = new DBRepository();
        }

        // GET api/docuemntrow/5
        public IEnumerable<DocumentRow> Get([FromUri]Document doc)
        {
            return dbRepository.GetDocumentRows(doc.Number, doc.Date);
        }

        // POST api/docuemntrow  JToken json
        public string Post([FromBody]DocumentRow docRow)
        {
            return dbRepository.AddDocumentRow(docRow);
        }
    }
}
