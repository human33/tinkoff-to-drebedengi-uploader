using System.Collections.Generic;

namespace T2DUploader.Utility.Drebedengi
{
    public class Database
    {
        public List<Currency> Currencies { get; set; } = new();
        public List<DrebedengiObject> Objects { get; set; } = new();
        public List<Record> Records { get; set; } = new();
    }
}