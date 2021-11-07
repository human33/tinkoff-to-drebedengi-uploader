namespace T2DUploader.Services
{
    public class UploaderOptions
    {
        public Utility.IFileInfo DrebedengiDump {get;set;} = null!;
        public string? OutputFilePath {get;set;}
    }
}