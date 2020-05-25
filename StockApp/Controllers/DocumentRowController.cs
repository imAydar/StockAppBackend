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
namespace StockApp.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DocumentRowController : ApiController
    {
        // GET api/docuemntrow/5
        public IEnumerable<DocumentRow> Get([FromUri]Document doc)
        {
            return DBConnection.GetRows(doc.Number, doc.Date);
        }

        // POST api/docuemntrow  JToken json
        public string Post([FromBody]DocumentRow Row)
        {
            return DBConnection.AddRow(Row);
        }
    }
}
