using System.Collections.Generic;
using System.Threading.Tasks;
using T2DUploader.Model;

namespace T2DUploader.Services
{
    public interface IUploader
    {
        public Task Upload(Expense expense);
    }
}