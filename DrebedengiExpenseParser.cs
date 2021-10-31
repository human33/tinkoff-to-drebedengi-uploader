using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace T2DUploader
{
    public class DrebedengiExpenseParser
    {
        private static string DREBEDENGI_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static async Task<List<Expense>> ParseList(FileInfo dump)
        {
            List<Expense> expenses = new();
            
            await foreach (Expense expense in Parse(dump))
            {
                expenses.Add(expense);
            }

            return expenses;
        }
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
            string[] cols = csvLine.Split(';');
            var money = decimal.Parse(cols[0]);
            var currency = cols[1];
            var category = cols[2];
            var account = cols[3];
            var date = cols[4];
            var comment = cols[5];
            
            // todo: detect header line
            if (date == "")
            {
                throw new ArgumentException("Passed header line as argument"); //# skip the header line
            }

            CultureInfo provider = CultureInfo.InvariantCulture; // I don't use culture specific format, so it's ok
            var parsedDate = DateTime.ParseExact(date, DREBEDENGI_DATE_FORMAT, provider);

            return new Expense(
                money: money,
                currency: currency,
                category: category,
                account: account,
                date: parsedDate,
                comment: comment,
                user: null,
                expenseGroup: null
            );
        }
    }
}