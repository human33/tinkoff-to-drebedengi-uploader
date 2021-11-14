using System.Collections.Generic;

namespace T2DUploader.Utility.Drebedengi
{
    public class Database
    {
        public Dictionary<long, Currency> Currencies { get; set; } = new();
        public Dictionary<long, DrebedengiObject> Objects { get; set; } = new();
        public List<Record> Records { get; set; } = new();
    }
}