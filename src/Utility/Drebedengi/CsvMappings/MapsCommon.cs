using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace T2DUploader.Utility.Drebedengi.CsvMappings
{
    public static class MapsCommon
    {
        public static ConvertFromString<bool> BooleanConverter(int index)
        {
            return (ConvertFromStringArgs row) => row.Row.GetField(index) == "t";
        }
        public static ConvertFromString<bool?> NullableBooleanConverter(int index)
        {
            return (ConvertFromStringArgs row) =>
            {
                string? field = row.Row.GetField(index);
                
                if (string.IsNullOrWhiteSpace(field))
                {
                    return null;
                }
                
                return field == "t";
            };
        }

        private static string DREBEDENGI_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static MemberMap<TClass, DateTime> MapDateTime<TClass>(this MemberMap<TClass,DateTime> map, 
            int index)
        {
            ConvertFromString<DateTime> f = (ConvertFromStringArgs row) =>
            {
                string s = row.Row.GetField(index);
                CultureInfo provider = CultureInfo.InvariantCulture; // I don't use culture specific format, so it's ok
                var parsedDate = DateTime.ParseExact(s, DREBEDENGI_DATE_FORMAT, provider);
                return parsedDate;
            };
            
            return map.Convert(f);
        }
    }
}