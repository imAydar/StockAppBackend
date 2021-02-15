using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockApp.Models;
using StockApp.Helpers;

namespace StockApp.Repositories
{
    public class DBRepository : IDBRepository
    {
        public IEnumerable<WareInfoDTO> FindWares(string pattern)
        {
            if (IsBarcode(pattern))
                return DBConnection.GetWare(pattern);
            else
                return DBConnection.GetWaresInfo(pattern);
        }

        public IEnumerable<WareInfo> GetWareInfo(string barcode)
        {
            return DBConnection.GetWareInfo(barcode);
        }
        
        private bool IsBarcode(string pattern)
        {
            return pattern.All(x => Char.IsDigit(x))
                && (pattern.Length == 13);//barcode length is usually 13
        }


        #region Documents

        public IEnumerable<Document> GetLastDocuments()
        {
            return DBConnection.GetLastDocuments(20);
        }

        public IEnumerable<DocumentRow> GetDocumentRows(string docNumber, DateTime docDate)
        {
            return DBConnection.GetRows(docNumber, docDate);
        }

        public string AddDocumentRow(DocumentRow docRow)
        {
            return DBConnection.AddRow(docRow);
        }

        #endregion
    }
}