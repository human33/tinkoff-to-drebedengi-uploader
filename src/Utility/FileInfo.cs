using System.IO;

namespace T2DUploader.Utility
{
    public class FileInfo: IFileInfo
    {
        private readonly System.IO.FileInfo _fileInfo;

        public FileInfo(System.IO.FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public Stream OpenRead()
        {
            return _fileInfo.OpenRead();
        }

        public string FullName => _fileInfo.FullName;
    }
}