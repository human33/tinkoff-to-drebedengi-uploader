using System;
using System.IO;
using System.Threading.Tasks;

namespace T2DUploader
{
    public class UploaderOptions 
    {
        public FileInfo? tinkoffDump {get;set;}
        public FileInfo? drebedengiDump {get;set;}
        public string? o {get;set;}
    }
}