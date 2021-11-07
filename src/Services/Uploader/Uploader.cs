using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using T2DUploader.Model;

namespace T2DUploader.Services
{
    class Uploader : IUploader
    {
        private readonly UploaderOptions _options;
        private readonly IUserInterface _userInterface;

        public Uploader(UploaderOptions options, IUserInterface userInterface)
        {
            _options = options;
            _userInterface = userInterface;
        }

        private List<Expense>? _drebedengiExpenses = null;

        private async Task<List<Expense>> GetDrebedengiExpenses()
        {
            if (_drebedengiExpenses == null)
            {
                await using (Stream drebedengiDumpStream = _options.DrebedengiDump.OpenRead())
                {
                    _drebedengiExpenses = await DrebedengiExpenseParser.ParseList(drebedengiDumpStream);
                }
            }

            return _drebedengiExpenses;
        }

        public async Task Upload(Expense expense)
        {
            await using System.IO.TextWriter outStream = _options.OutputFilePath != null 
                ? new System.IO.StreamWriter(_options.OutputFilePath, append:true)
                : System.Console.Out;

            List<Expense> expensesInDrebedengi = await GetDrebedengiExpenses();
            Expense? alikeExpense = expensesInDrebedengi.FirstOrDefault(e => e.Like(expense));

            if (alikeExpense != null)
            {
                if (! await _userInterface.ShouldUploadAlike(fromDrebedengi: alikeExpense, expense))
                {
                    return;
                }
            }

            string csvLine = DrebedengiExpenseExporter.ExportAsCsvLine(expense);
            await outStream.WriteLineAsync(csvLine);
        }
    }
}