using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockApp.Models
{
    public class Document
    {
        public string Number { get; set; }//code of docuemnt
        public DateTime Date { get; set; }
        public string Owner { get; set; }
        public string Comment { get; set; }
    }
    public class DocumentDto
    {
        public string Number { get; set; }//code of docuemnt
        public string Date { get; set; }
        public string Owner { get; set; }

        public Document ToDocument()
        {
            return new Document()
            {
                Number = this.Number,
                Owner = this.Owner,
                Date = DateTime.Parse(this.Date)
            };
        }
    }
}