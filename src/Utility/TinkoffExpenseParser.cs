using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            using Stream stream = dump.OpenRead();
            
            // because tinkoff encodes dump in this encoding 
            using StreamReader reader = new(stream, Encoding.GetEncoding("windows-1251")); 
            
            // read header and ignore it
            // ReSharper disable once UnusedVariable
            string? header = await reader.ReadLineAsync();

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();

                if (line == null)
                {
                    break;
                }

                ParseResult parsed = Parse(line);

                if (parsed.Status != ParseStatus.OK) {
                    continue;
                }

                yield return parsed.Expense!;
            }
        }
        
        public enum ParseStatus {
            OK,
            UnableToParse,
            FailedOperation
        }
        public record ParseResult(ParseStatus Status, Expense? Expense);

        public static ParseResult Parse(string csvLine)
        {
            // todo: parse unprepared csv as well
            // example line format:
            //"Дата операции";"Дата платежа";"Номер карты";"Статус";"Сумма операции";"Валюта операции";"Сумма платежа";"Валюта платежа";"Кэшбэк";"Категория";"MCC";"Описание";"Бонусы (включая кэшбэк)";"Округление на инвесткопилку";"Сумма операции с округлением"
            //"21.11.2020 21:59:15";"21.11.2020";"*7212";"OK";"-1000,00";"RUB";"-1000,00";"RUB";"";"Переводы/иб";"";"Перевод между счетами";"0,00";"0,00";"1000,00"


            string[] cols = csvLine.Split(';').Select(s => s.Trim('"')).ToArray();
            string date = cols[0];

            string operationStatus = cols[3];
            if (operationStatus != "OK") {
                return new ParseResult(ParseStatus.FailedOperation, null);
            }

            string money = cols[6];
            string currency = cols[7];
            string category = cols[9];
            string desc = cols[11];

            if (date == "Дата операции")
            {
                throw new ArgumentException("Passed header line as argument"); //# skip the header line
            }
            
            // decimal point in drebedengi is '.', but it's ',' in tinkoff
            money = money.Replace(',', '.');
            decimal moneyNum = Decimal.Parse(money);

            currency = currency switch
            {
                "RUB" => "Руб", // use drebedengi currency format
                "USD" => "USD",
                _ => currency
            };

            CultureInfo provider = CultureInfo.InvariantCulture; // I don't use culture specific format, so it's ok
            var parsedDate = DateTime.ParseExact(date, TINKOFF_DATE_FORMAT, provider);

            return new ParseResult(
                ParseStatus.OK,
                new Expense(
                    money: moneyNum,
                    currency: currency,
                    category: category,
                    account: null,
                    date: parsedDate,
                    comment: desc,
                    user: null,
                    expenseGroup: null
                )
            );
        }
    }
}