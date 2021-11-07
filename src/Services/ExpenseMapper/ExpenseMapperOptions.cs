using System.Collections.Generic;

namespace T2DUploader.Services.ExpenseMapper
{
    public class ExpenseMapperOptions 
    {
        public Utility.IFileInfo TinkoffDump {get;set;} = null!;
        public Dictionary<string, string> DescriptionToAccount { get; set; } = new();
    }
}