using System;
using System.Collections.Generic;
using T2DUploader.Model;

namespace T2DUploader.Services.ExpenseMapper
{
    public class ExpenseMapper : IExpenseMapper
    {
        private readonly ExpenseMapperOptions _options;
        private readonly IUserInterface _interface;

        public ExpenseMapper(ExpenseMapperOptions options, IUserInterface ui)
        {
            _options = options;
            _interface = ui;
        }

        public async IAsyncEnumerable<Expense> Map() 
        {
            Console.WriteLine($"tinkoffDump is: {_options.TinkoffDump.FullName}");

            // parse expenses
            IAsyncEnumerable<Expense> tinkoffExpenses = TinkoffExpenseParser.Parse(_options.TinkoffDump);
            

            // and find out what is already in drebedengi
            await foreach (Expense expense in tinkoffExpenses)
            {
                if (string.IsNullOrWhiteSpace(expense.Account) && 
                    _options.DescriptionToAccount.TryGetValue(expense.Comment, out string? account))
                {
                    var modifiedExpense = expense with
                    {
                        Account = account
                    };
                    
                    yield return modifiedExpense;
                    continue;
                }

                yield return expense;
            }
        }
    }
}