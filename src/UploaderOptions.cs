using System;
using System.IO;
using System.Threading.Tasks;

namespace T2DUploader
{
    public class UploaderOptions 
    {
        public Utility.IFileInfo tinkoffDump {get;set;}
        public Utility.IFileInfo drebedengiDump {get;set;}
        public string? o {get;set;}
    }
}