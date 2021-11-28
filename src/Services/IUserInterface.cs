using System.Threading.Tasks;
using T2DUploader.Model;

namespace T2DUploader.Services
{
    public interface IUserInterface 
    {
        Task<bool> ShouldUploadAlike(Expense fromDrebedengi, Expense fromTinkoff);
        Task ThereIsNoPairedExpenseFor(Expense expense);
    }
}