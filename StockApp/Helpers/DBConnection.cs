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
        private static dynamic connection1c = null;

        static DBConnection()
        {
                Connect();
        }

        private static void Connect()
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

        public static IEnumerable<Document> GetLastDocuments(int amount = 20)
        {
            dynamic dataArray1C = connection1c.Документы.ИнвентаризацияТоваровНаСкладе.Выбрать(null, null, null, "Дата убыв");//Get selection of last documents

            int ind = 0;
            while (dataArray1C.Следующий() == true)
            {
                yield return new Document()
                {
                    Number = dataArray1C.Номер,
                    Date = dataArray1C.Дата,
                    Owner = dataArray1C.Ответственный.Наименование,
                    Comment = dataArray1C.Комментарий
                };
                ind++;
                if (ind >= amount) break;
            }
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

        public static string AddRow(DocumentRow Row)
        {
            dynamic docs = connection1c.Документы.ИнвентаризацияТоваровНаСкладе;
            dynamic comDocument = docs.НайтиПоНомеру(Row.Document.Number, Row.Document.Date);
            dynamic uid = connection1c.NewObject("УникальныйИдентификатор", (comDocument.УникальныйИдентификатор()));
            dynamic comDocJbj = docs.ПолучитьСсылку(uid).ПолучитьОбъект();

            dynamic ware = GetWare(Row.Barcode);
            if (ware == null)
                return "Ошибка: не была найдена номенклатура";

            if (AddRow(ware, comDocJbj))
                return ware.Код + ";" + ware.Наименование;

            return "Ошибка: не удалось добавить номенклатуру";
        }

        private static dynamic GetWare(string Barcode)
        {
            dynamic query = connection1c.NewObject("Запрос");
            query.Текст = @"ВЫБРАТЬ
	            Штрихкоды.Владелец как ссылка
            ИЗ
	            РегистрСведений.Штрихкоды КАК Штрихкоды
            ГДЕ
	            Штрихкоды.Штрихкод = &Штрихкод";
            query.УстановитьПараметр("Штрихкод", Barcode);
            dynamic req = query.Выполнить().Выбрать();
            while (req.Следующий())
            {
                return req.Ссылка;
            }
            return null;
        }

        private static Boolean AddRow(dynamic Ware, dynamic Doc)
        {
            dynamic str = Doc.Товары.Найти(Ware, "Номенклатура");
            if (str == null)
            {
                str = Doc.Товары.Добавить();
                str.Номенклатура = Ware;
                str.Количество = 0;
                str.Коэффициент = 1;
                str.ЕдиницаИзмерения = Ware.ЕдиницаХраненияОстатков;
            }
            str.Количество += 1;
            try
            {
                Doc.Записать();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

    }
}