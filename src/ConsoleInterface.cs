using System;
using System.Threading.Tasks;

namespace T2DUploader
{
    class ConsoleInterface : IUserInterface
    {
        // private object _lock = new object();

        public Task<bool> Confirm(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("type y(yes) or n(no) to confirm or deny");
            ConsoleKeyInfo key = Console.ReadKey();
            return Task.FromResult(key.KeyChar == 'y');
        }
    }
}