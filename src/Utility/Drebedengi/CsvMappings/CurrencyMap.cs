using System;
using CsvHelper;
using CsvHelper.Configuration;

namespace T2DUploader.Utility.Drebedengi.CsvMappings
{
    // ReSharper disable once UnusedType.Global
    public sealed class CurrencyMap: ClassMap<Currency>
    {
        public CurrencyMap()
        {
            Map(c => c.Id).Index(0);
            Map(c => c.Name).Index(1);
            Map(c => c.Course).Index(2);
            Map(c => c.Code).Index(3);
            Map(c => c.IsAutoUpdate).Convert(MapsCommon.BooleanConverter(index: 4));
            Map(c => c.IsHidden).Convert(MapsCommon.BooleanConverter(index: 5));
            Map(c => c.IsDefault).Convert(MapsCommon.BooleanConverter(index: 6));
        }

    }
}