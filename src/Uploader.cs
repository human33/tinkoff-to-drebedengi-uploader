using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace T2DUploader
{
    public class Uploader 
    {
        private readonly UploaderOptions _options;
        private readonly IUserInterface _interface;

        public Uploader(UploaderOptions options, IUserInterface ui)
        {
            _options = options;
            _interface = ui;
        }

        public async Task Upload() 
        {
            Console.WriteLine($"tinkoffDump is: {_options.tinkoffDump?.FullName}");
            Console.WriteLine($"drebedengiDump is: {_options.drebedengiDump?.FullName}");
            Console.WriteLine($"o is: {_options.o ?? "null"}");

            Debug.Assert(_options.tinkoffDump != null, nameof(_options.tinkoffDump) + " != null");
            Debug.Assert(_options.drebedengiDump != null, nameof(_options.drebedengiDump) + " != null");
            
            
            // parse expenses
            IAsyncEnumerable<Expense> tinkoffExpenses = TinkoffExpenseParser.Parse(_options.tinkoffDump);
            List<Expense> drebedengiExpenses;

            await using (Stream drebedengiDumpStream = _options.drebedengiDump.OpenRead())
            {
                drebedengiExpenses = await DrebedengiExpenseParser.ParseList(drebedengiDumpStream);
            }

            await using System.IO.TextWriter outStream = _options.o != null 
                ? new System.IO.StreamWriter(_options.o)
                : System.Console.Out;

            // and find out what is already in drebedengi
            await foreach (Expense expense in tinkoffExpenses)
            {
                // there is an expense like read from tinkoff
                var alikeExpenseFromDrebedengi = drebedengiExpenses.FirstOrDefault(e => e.Like(expense));
                    
                if (alikeExpenseFromDrebedengi != null)
                {
                    
                    if (! await _interface.ShouldUploadAlike(alikeExpenseFromDrebedengi, expense)) 
                    {
                        continue;
                    }
                }

                string csvLine = DrebedengiExpenseExporter.ExportAsCsvLine(expense);
                await outStream.WriteLineAsync(csvLine);
            }
        }
    }
}