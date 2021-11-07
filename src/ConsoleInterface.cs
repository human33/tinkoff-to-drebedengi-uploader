using System;
using System.Threading.Tasks;

namespace T2DUploader
{
    class ConsoleInterface : IUserInterface
    {
        // private object _lock = new object();

        public Task<bool> ShouldUploadAlike(Expense fromDrebedengi, Expense fromTinkoff)
        {
            var message = $"Expense for {fromTinkoff.Date.ToString("o")} "+
                $"with sum {fromTinkoff.Money} was found in Drebedengi " +
                $"(for {fromTinkoff.Date.ToString("o")} with sum {fromDrebedengi.Money}" +
                $", upload anyway?";

            Console.WriteLine(message);
            Console.WriteLine("type y(yes) or n(no) to confirm or deny");
            ConsoleKeyInfo key = Console.ReadKey();
            return Task.FromResult(key.KeyChar == 'y');
        }

    }
}