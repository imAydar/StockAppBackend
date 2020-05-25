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
       /* public IEnumerable<DocumentDTO> get()
        {
            if (Environment.Is64BitProcess)
                return new List<DocumentDTO>() { new DocumentDTO()
                {Owner = "e,fyjtlthmvj" }
                };
            dynamic connection1c;
            COMConnector connector = new COMConnector();
            String connectStr = @"Srvr=""192.168.200.110"";Ref=""ut82"";Usr=""Галиев Айдар"";PWD=""10079319"";";
            connector.PoolCapacity = 10;
            connector.PoolTimeout = 60;
            connector.MaxConnections = 10;
            //    Console.WriteLine(connectStr);
            connection1c = connector.Connect(connectStr);
            dynamic dataArray1C = connection1c.Документы.ИнвентаризацияТоваровНаСкладе.Выбрать();
            //var document = new DocumentDTO();
            var documents = new List<DocumentDTO>();
            int ind = 0;
            while (dataArray1C.Следующий == true)
            {
                //grid.Rows.Add(dataArray1C.Наименование);
                //ret += dataArray1C.Номер;
                documents.Add(new DocumentDTO()
                {
                    Number = dataArray1C.Номер,
                    Date = dataArray1C.Дата,
                    Owner = dataArray1C.Ответственный.Наименование
                });
                ind++;
                if (ind > 9) break;
            }
            return documents;
        }*/

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