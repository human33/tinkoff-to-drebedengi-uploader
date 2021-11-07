using System;
using System.Threading.Tasks;

namespace T2DUploader
{
    public interface IUserInterface 
    {
        Task<bool> ShouldUploadAlike(Expense fromDrebedengi, Expense fromTinkoff);
    }
}