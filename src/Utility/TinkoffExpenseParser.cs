using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using T2DUploader.Model;
using T2DUploader.Utility;

namespace T2DUploader
{
    public static class TinkoffExpenseParser
    {
        static TinkoffExpenseParser()
        {
            // otherwise windows-1251 encoding would be unavailable
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        
        private static string TINKOFF_DATE_FORMAT = "dd.MM.yyyy HH:mm:ss";

        public static async IAsyncEnumerable<Expense> Parse(IFileInfo dump)
        {
            Console.Out.WriteLine("Open file for reading");
            using Stream stream = dump.OpenRead();
            
            // because tinkoff encodes dump in this encoding 
            using StreamReader reader = new(stream, Encoding.GetEncoding("windows-1251")); 
            Console.Out.WriteLine("Reading header");
            
            // read header and ignore it
            // ReSharper disable once UnusedVariable
            string? header = await reader.ReadLineAsync();
            
            Console.Out.WriteLine("Header read");

            while (!reader.EndOfStream)
            {
                Console.Out.WriteLine("Read line");
                string? line = await reader.ReadLineAsync();

                if (line == null)
                {
                    break;
                }

                Console.Out.WriteLine("Parse line");
                Expense parsed = Parse(line);
                yield return parsed;
            }
        }
        
        public static Expense Parse(string csvLine)
        {
            // todo: parse unprepared csv as well
            // example line format:
            // "Дата операции";"Дата платежа";"Номер карты";"Статус";"Сумма операции";"Валюта операции";"Сумма платежа";"Валюта платежа";"Кэшбэк";"Категория";"MCC";"Описание";"Бонусы (включая кэшбэк)";"Округление на инвесткопилку";"Сумма операции с округлением"
            // "07.11.2021 15:24:34";"07.11.2021";"*9894";"OK";"-1014,46";"RUB";"-1014,46";"RUB";"";"Супермаркеты";"5411";"ВкусВилл";"10,00";"0,00";"1014,46"
            
            string[] cols = csvLine.Split(';');
            var date = cols[0];
            var money = cols[1];
            var currency = cols[2];
            var category = cols[3];
            var desc = cols[4];
            var account = cols.Length > 5 
                ? cols[5]
                : null; // account name from drebedengi

            if (date == "Дата операции")
            {
                throw new ArgumentException("Passed header line as argument"); //# skip the header line
            }
            
            // decimal point in drebedengi is '.', but it's ',' in tinkoff
            money = money.Replace(',', '.');
            decimal moneyNum = Decimal.Parse(money);

            currency = currency switch
            {
                "RUB" => "руб", // use drebedengi currency format
                "USD" => "USD",
                _ => currency
            };

            CultureInfo provider = CultureInfo.InvariantCulture; // I don't use culture specific format, so it's ok
            var parsedDate = DateTime.ParseExact(date, TINKOFF_DATE_FORMAT, provider);

            return new Expense(
                money: moneyNum,
                currency: currency,
                category: category,
                account: account,
                date: parsedDate,
                comment: desc,
                user: null,
                expenseGroup: null
            );
        }
    }
}