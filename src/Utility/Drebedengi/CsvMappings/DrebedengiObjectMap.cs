using CsvHelper.Configuration;

namespace T2DUploader.Utility.Drebedengi.CsvMappings
{
    // ReSharper disable once UnusedType.Global
    public sealed class DrebedengiObjectMap: ClassMap<DrebedengiObject>
    {
        public DrebedengiObjectMap()
        {
            Map(o => o.Id).Index(0);
            Map(o => o.ParentId).Index(1);
            Map(o => o.Type).Index(2);
            Map(o => o.Name).Index(3);
            Map(o => o.UserId).Index(4);
            Map(o => o.IsCreditCard).Convert(MapsCommon.BooleanConverter(5));
            Map(o => o.IsHidden).Convert(MapsCommon.BooleanConverter(6));
            Map(o => o.IsForDuty).Convert(MapsCommon.BooleanConverter(7));
            Map(o => o.Sort).Index(8);
            Map(o => o.IconId).Index(9);
            Map(o => o.IsAutohide).Convert(MapsCommon.NullableBooleanConverter(10));
        }
    }
}