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
            
            // todo: notify about income entries (ignore them or upload as income?)
            // todo: ignore expenses with - and + entries for the same card (returns)

            Dictionary<string, Expense> toRemovePaired = new();

            // and find out what is already in drebedengi
            await foreach (Expense expense in tinkoffExpenses)
            {
                if (string.IsNullOrWhiteSpace(expense.Account) && 
                    _options.DescriptionToAccount.TryGetValue(expense.Comment, out string? account))
                {
                    if (account == "ignore_minus" && expense.Money < 0) 
                    {
                        continue;
                    }


                    if (account == "remove_paired") 
                    {
                        if (toRemovePaired.Remove(expense.Comment, out var pairedExpense)) 
                        {
                            if (pairedExpense.Money == -expense.Money) 
                            {
                                // it's totally ok
                                continue;   
                            }
                            else 
                            {
                                // it's a bad thing, means that there is no paired expense with opposite sign
                                await _interface.ThereIsNoPairedExpenseFor(pairedExpense);
                                break;
                            }
                        }
                        else 
                        {
                            toRemovePaired.Add(expense.Comment, expense);
                            continue;
                        }
                    }

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