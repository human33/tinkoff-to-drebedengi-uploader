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
                // await using (Stream drebedengiDumpStream = _options.DrebedengiDump.OpenRead())
                // {
                //     _drebedengiExpenses = await DrebedengiExpenseParser.ParseList(drebedengiDumpStream);
                // }

                var db = await Utility.Drebedengi.Parser.ParseExtendedFormat(_options.DrebedengiDump);

                _drebedengiExpenses = db.Records.Select(r => {
                    var currency = db.Currencies[r.CurrencyId];
                    var account = db.Objects[r.AccountId];

                    if (r.ObjectId == -1) {
                        return null;
                    }

                    var category = db.Objects[r.ObjectId];

                    if (category.Type != Utility.Drebedengi.ObjectType.ExpenseCategory) {
                        return null;
                    }

                    return new Expense(
                        money: r.Sum / 100, // because records sum is smalles currency representation 
                        currency: currency.Name,
                        category: category.Name,
                        account: account.Name,
                        date: r.Date,
                        comment: r.Comment,
                        user: r.UserId?.ToString(),
                        expenseGroup: r.GroupId?.ToString()
                    );
                })
                .Where(e => e != null)
                .ToList()!;
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
                // if it's exact match - ignore it
                if (MostlyEqual(expense, alikeExpense)) 
                {
                    return;
                }

                if (! await _userInterface.ShouldUploadAlike(fromDrebedengi: alikeExpense, expense))
                {
                    return;
                }
            }

            string csvLine = DrebedengiExpenseExporter.ExportAsCsvLine(expense);
            await outStream.WriteLineAsync(csvLine);
        }

        protected virtual bool MostlyEqual(Expense expense1, Expense expense2)
        {            
            // ignore drebedengi user and expense group because the don't matter here
            return expense1.Money        == expense2.Money &&
                   expense1.Currency     == expense2.Currency &&
                   expense1.Category     == expense2.Category &&
                //   expense1.Account      == expense2.Account &&
                   expense1.Date         == expense2.Date &&
                   expense1.Comment      == expense2.Comment;
        }
    }
}