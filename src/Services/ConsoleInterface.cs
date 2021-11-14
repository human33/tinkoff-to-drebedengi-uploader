using System;
using System.Threading.Tasks;
using T2DUploader.Model;

namespace T2DUploader.Services
{
    class ConsoleInterface : IUserInterface
    {
        // private object _lock = new object();

        public Task<bool> ShouldUploadAlike(Expense fromDrebedengi, Expense fromTinkoff)
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

            Console.WriteLine(message);
            Console.WriteLine("type y(yes) or n(no) to confirm or deny");
            ConsoleKeyInfo key = Console.ReadKey();
            return Task.FromResult(key.KeyChar == 'y');
        }

    }
}