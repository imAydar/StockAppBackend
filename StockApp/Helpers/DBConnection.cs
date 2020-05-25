using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using V83;
using StockApp.Models;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;

namespace StockApp.Helpers
{
    public static class DBConnection
    {

        public static dynamic connection1c;
        static DBConnection()
        {
            if (connection1c == null)
            {
                var connector = new COMConnector();
                var settings = ConfigurationManager.ConnectionStrings
                                               .Cast<ConnectionStringSettings>()
                                               .Where(c => c.Name == "V83").FirstOrDefault();
                String connectStr = settings.ConnectionString;
                connector.PoolCapacity = 10;
                connector.PoolTimeout = 60;
                connector.MaxConnections = 12;
                connection1c = connector.Connect(connectStr);
            }
        }


        public static List<Document> GetLastDocuments(int amount = 20)
        {
                dynamic dataArray1C = connection1c.Документы.ИнвентаризацияТоваровНаСкладе.Выбрать(null, null, null, "Дата убыв");
                var documents = new List<Document>();
                int ind = 0;
                while (dataArray1C.Следующий() == true)
                {
                    documents.Add(new Document()
                    {
                        Number = dataArray1C.Номер,
                        Date = dataArray1C.Дата,
                        Owner = dataArray1C.Ответственный.Наименование,
                        Comment = dataArray1C.Комментарий
                    });
                    ind++;
                    if (ind >= amount) break;
                }
                return documents;
            
        }

        public static IEnumerable<DocumentRow> GetRows(string DocumentCode, DateTime DocumentDate)
        {
            
            dynamic comDocument = connection1c.Документы.ИнвентаризацияТоваровНаСкладе.НайтиПоНомеру(DocumentCode, DocumentDate);
            foreach (var row in comDocument.Товары)
            {
                yield return new DocumentRow()
                {
                    Code = row.Номенклатура.Код,
                    Quantity = row.Количество,
                    WareName = row.Номенклатура.Наименование,
                    HasToBeQuantity = row.КоличествоУчет
                };
            }
        }

        public static string AddRow(DocumentRow row)
        {
            dynamic docs = connection1c.Документы.ИнвентаризацияТоваровНаСкладе;
            dynamic comDocument = docs.НайтиПоНомеру(row.Document.Number, row.Document.Date);

            dynamic uid = connection1c.NewObject("УникальныйИдентификатор", (comDocument.УникальныйИдентификатор()));
            dynamic comDocJbj = docs.ПолучитьСсылку(uid).ПолучитьОбъект();

            return comDocJbj.ДобавитьШтрихкод(row.Barcode);
        }
 
    }
}