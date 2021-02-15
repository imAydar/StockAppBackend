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
    public class FindController : ApiController
    {
        public IDBRepository DBRepository;

        public FindController()
        {
            DBRepository = new DBRepository();
        }


        public IEnumerable<WareInfoDTO> Get(string id)
        {
            return DBRepository.FindWares(id);
        }
    }
}
