using System;
using System.Threading.Tasks;
using T2DUploader.Model;

namespace T2DUploader.Services
{
    class ConsoleInterface : IUserInterface
    {
        // private object _lock = new object();

        public async Task<bool> ShouldUploadAlike(Expense fromDrebedengi, Expense fromTinkoff)
        {
            // var message = $"Expense for {fromTinkoff.Date.ToString("o")} "+
            //     $"with sum {fromTinkoff.Money} was found in Drebedengi " +
            //     $"(for {fromDrebedengi.Date.ToString("o")} with sum {fromDrebedengi.Money}" +
            //     $", upload anyway?";

            var message = "Similar expense was found in Drebedengi:\n" +
                "\t|Tinkoff|\tDrebedengi\n" +
                $"Date:\t{fromTinkoff.Date.ToString("o")}\t{fromDrebedengi.Date.ToString("o")}\n" +
                $"Money:\t{fromTinkoff.Money}\t{fromDrebedengi.Money}\n" +
                $"Account:\t{fromTinkoff.Account}\t{fromDrebedengi.Account}\n" +
                $"Category:\t{fromTinkoff.Category}\t{fromDrebedengi.Category}\n" +
                $"Comment:\t{fromTinkoff.Comment}\t{fromDrebedengi.Comment}\n" +
                $"Currency:\t{fromTinkoff.Currency}\t{fromDrebedengi.Currency}\n" +
                $"User:\t{fromTinkoff.User ?? "None"}\t{fromDrebedengi.User ?? "None"}\n" +
                $"ExpenseGroup:\t{fromTinkoff.ExpenseGroup ?? "None"}\t{fromDrebedengi.ExpenseGroup ?? "None"}\n" +
                "Upload anyway?";

            await Console.Out.WriteLineAsync(message);
            await Console.Out.WriteLineAsync("type y(yes) or n(no) to confirm or deny");
            ConsoleKeyInfo key = Console.ReadKey();
            return key.KeyChar == 'y';
        }

        public async Task ThereIsNoPairedExpenseFor(Expense expense)
        {
            string message = "There is no paired expense for expense with sum: " + 
                expense.Money + "; and desc: " + expense.Comment + ".";
            await Console.Out.WriteLineAsync(message);
        }
        
        public async Task FoundCurrencyExchange(Expense nextExpense, Expense expense)
        {
            // use -expense.Money, because it's a withdrawal from one account to another inside the bank  
            string message = "Notice! Currency exchange found " + expense.Date + "; " + expense.Money +" "+ expense.Currency + " → " + 
              (-nextExpense.Money) +" "+ nextExpense.Currency + "; Exchange rate: " + (-Math.Round(nextExpense.Money / expense.Money , 2));
            await Console.Out.WriteLineAsync(message);
        }
    }
}