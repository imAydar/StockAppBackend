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
            var settings = ConfigurationManager.ConnectionStrings
                                           .Cast<ConnectionStringSettings>()
                                           .Where(c => c.Name == "V83").FirstOrDefault();
            String connectionStr = settings.ConnectionString;

            COMConnector connector = new COMConnector()
            {
                PoolCapacity = 10,
                PoolTimeout = 60,
                MaxConnections = 12
            };
            connection1c = connector.Connect(connectionStr);
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
                if (ind == amount - 1) break;
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
            {
                return "Ошибка: не была найдена номенклатура";
            }

            if (AddRow(ware, comDocJbj))
            {
                return ware.Код + ";" + ware.Наименование;
            }

            return "Ошибка: не удалось добавить номенклатуру";
        }

        public static IEnumerable<WareInfoDTO> GetWare(string Barcode)
        {
            dynamic query = connection1c.NewObject("Запрос");
            query.Текст = @"ВЫБРАТЬ
	            Штрихкоды.Владелец.Наименование как Наименование
            ИЗ
	            РегистрСведений.Штрихкоды КАК Штрихкоды
            ГДЕ
	            Штрихкоды.Штрихкод = &Штрихкод";

            query.УстановитьПараметр("Штрихкод", Barcode);
            dynamic req = query.Выполнить().Выбрать();
            while (req.Следующий())
            {
                yield return new WareInfoDTO() { Name = req.Наименование, Barecode = Barcode };
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns>Collection of WareInfo objects, empty collection if theres none</returns>
        public static IEnumerable<WareInfo> GetWareInfo(string Barcode)
        {
            dynamic query = connection1c.NewObject("Запрос");
            query.Text = @"ВЫБРАТЬ
	ТоварыВРозницеОстатки.Склад.Наименование как склад,
	ТоварыВРозницеОстатки.Номенклатура.Наименование как товар,
	ТоварыВРозницеОстатки.КоличествоОстаток как остаток,
	ЦеныАТТСрезПоследних.Цена как цена
ИЗ
	РегистрНакопления.ТоварыВРознице.Остатки КАК ТоварыВРозницеОстатки
		ВНУТРЕННЕЕ СОЕДИНЕНИЕ РегистрСведений.Штрихкоды КАК Штрихкоды
		ПО ТоварыВРозницеОстатки.Номенклатура = Штрихкоды.Владелец
		ВНУТРЕННЕЕ СОЕДИНЕНИЕ РегистрСведений.ЦеныАТТ.СрезПоследних КАК ЦеныАТТСрезПоследних
		по ТоварыВРозницеОстатки.Номенклатура = ЦеныАТТСрезПоследних.Номенклатура 
ГДЕ " +
    "Штрихкоды.Штрихкод в (\"" + Barcode + "\") " +
    @"И ТоварыВРозницеОстатки.Склад = ЦеныАТТСрезПоследних.Склад 
                упорядочить по товар";
            dynamic req = query.Выполнить().Выбрать();
            while (req.Следующий())
            {
                yield return new WareInfo {
                    Barecode = Barcode,
                    Name = req.Товар,
                    Store = req.склад,
                    Remain = Convert.ToString(req.остаток) + " шт.",
                    Price = Convert.ToString(req.Цена) + " руб."
                };
            }
        }
       
        /// <summary>
        /// returns base info of particular ware 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Collection of WareInfo objects, empty collection if theres none</returns>
        public static IEnumerable<WareInfoDTO> GetWaresInfo(string Name)
        {
            Name = "%" + Name.Replace(" ", "%") + "%";

            dynamic query = connection1c.NewObject("Запрос");
            string text = @"ВЫБРАТЬ
	               	ТоварыВРозницеОстатки.Номенклатура.наименование как наименование,
	               	ТоварыВРозницеОстатки.Номенклатура.Родитель как Родитель,
	               	ТоварыВРозницеОстатки.Номенклатура.Код как Код,
	               	ТоварыВРозницеОстатки.Номенклатура.Артикул как Артикул,
	               	ТоварыВРозницеОстатки.Номенклатура.НоменклатурнаяГруппа КАК НомГруппа,
                    Штрихкоды.Штрихкод как штрихкод
	               ИЗ
	               	РегистрНакопления.ТоварыВРознице.Остатки(, Номенклатура.Наименование ПОДОБНО """ + Name + @""") КАК ТоварыВРозницеОстатки
                        левое СОЕДИНЕНИЕ РегистрСведений.Штрихкоды КАК Штрихкоды
		            ПО ТоварыВРозницеОстатки.Номенклатура = Штрихкоды.Владелец
	               ГДЕ
	               	ТоварыВРозницеОстатки.КоличествоОстаток <> 0
	               УПОРЯДОЧИТЬ ПО
	               	ТоварыВРозницеОстатки.Номенклатура.Наименование";
            query.Text = text;

            dynamic req = query.Выполнить().Выбрать();
            while (req.Следующий())
            {
                yield return new WareInfoDTO()
                {
                    Name = req.наименование,
                    Barecode = req.штрихкод
                };
            }
        }

    }
}