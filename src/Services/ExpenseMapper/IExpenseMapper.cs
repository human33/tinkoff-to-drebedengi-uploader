using System.Collections.Generic;
using T2DUploader.Model;

namespace T2DUploader.Services.ExpenseMapper
{
    public interface IExpenseMapper
    {
        IAsyncEnumerable<Expense> Map();
    }
}