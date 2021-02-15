using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockApp.Models;

namespace StockApp.Repositories
{
    public interface IDBRepository
    {

        IEnumerable<WareInfoDTO> FindWares(string pattern);
        IEnumerable<WareInfo> GetWareInfo(string Barcode);

        IEnumerable<Document> GetLastDocuments();
        IEnumerable<DocumentRow> GetDocumentRows(string number, DateTime date);
        String AddDocumentRow(DocumentRow documentRow);


    }
}