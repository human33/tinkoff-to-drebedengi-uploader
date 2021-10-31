using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace T2DUploader
{
    public class TinkoffExpenseParser
    {
        private static string TINKOFF_DATE_FORMAT = "dd.MM.yyyy HH:mm:ss";

        public static async IAsyncEnumerable<Expense> Parse(FileInfo dump)
        {
            using Stream stream = dump.OpenRead();
            using StreamReader reader = new(stream);
                
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

                Expense parsed = Parse(line);
                yield return parsed;
            }
        }
        
        public static Expense Parse(string csvLine)
        {
            // todo: parse unprepared csv as well
            
            string[] cols = csvLine.Split(';');
            var date = cols[0];
            var money = cols[1];
            var currency = cols[2];
            var category = cols[3];
            var desc = cols[4];
            var account = cols[5]; // account name from drebedengi
            
            // decimal point in drebedengi is '.', but it's ',' in tinkoff
            money = money.Replace(',', '.');
            decimal moneyNum = Decimal.Parse(money);

            if (date == "Дата операции")
            {
                throw new ArgumentException("Passed header line as argument"); //# skip the header line
            }

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