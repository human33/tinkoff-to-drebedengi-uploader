using System.IO;

namespace T2DUploader.Utility
{
    public interface IFileInfo
    {
        public Stream OpenRead();
        public string FullName { get; }
    }
}