using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockApp.Models
{
    public class WareInfo
    {
        public string Barecode { get; set; }
        public string Name { get; set; }
        public string Store { get; set; }
        public string Price { get; set; }
        public string Remain { get; set; }
    }

    public class WareInfoDTO
    {
        public string Barecode { get; set; }
        public string Name { get; set; }
    }
}