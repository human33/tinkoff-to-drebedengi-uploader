using CsvHelper.Configuration;

namespace T2DUploader.Utility.Drebedengi.CsvMappings
{
    // ReSharper disable once UnusedType.Global
    public sealed class RecordMap: ClassMap<Record>
    {
        public RecordMap()
        {
            Map(r => r.Sum).Index(0);
            Map(r => r.CurrencyId).Index(1);
            Map(r => r.ObjectId).Index(2);
            Map(r => r.AccountId).Index(3);
            Map(r => r.Date).MapDateTime(4);
            Map(r => r.Comment).Index(5);
            Map(r => r.UserId).Index(6);
            Map(r => r.GroupId).Index(7);
        }
    }
}