using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockApp.Models
{
    public class DocumentRow
    {
        public string WareName { get; set; }
        public int Quantity { get; set; }
        public string Barcode { get; set; }
        public string Code { get; set; }
        public int HasToBeQuantity { get; set; }
        public Document Document { get; set; }
    }
}