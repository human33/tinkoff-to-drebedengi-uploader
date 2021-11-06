using System;
using System.Threading.Tasks;

namespace T2DUploader
{
    public interface IUserInterface 
    {
        Task<bool> Confirm(string message);
    }
}