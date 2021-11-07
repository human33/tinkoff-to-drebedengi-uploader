using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using T2DUploader.Model;

namespace T2DUploader
{
    public class DrebedengiExpenseParser
    {
        private static string DREBEDENGI_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static async Task<List<Expense>> ParseList(Stream dump)
        {
            List<Expense> expenses = new();
            
            await foreach (Expense expense in Parse(dump))
            {
                expenses.Add(expense);
            }

            return expenses;
        }
        public static async IAsyncEnumerable<Expense> Parse(Stream dump)
        {
            using StreamReader reader = new(dump);
            
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
            var category = cols[2].Trim().Trim('"');
            var account = cols[3];
            var date = cols[4];
            var comment = cols[5].Trim().Trim('"');
            
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