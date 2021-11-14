using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using T2DUploader.Utility.Drebedengi.CsvMappings;

namespace T2DUploader.Utility.Drebedengi
{
    public static class Parser
    {
        private enum ParsingStage
        {
            None,
            Currency,
            Objects,
            Records
        }
        
        public static async Task<Database> ParseExtendedFormat(FileInfo file)
        {
            Database result = new();
            await using Stream stream = file.OpenRead();
            using StreamReader reader = new(stream);
            
            using CsvReader csvReader = new(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false
            });

            csvReader.Context.RegisterClassMap<CurrencyMap>();
            csvReader.Context.RegisterClassMap<DrebedengiObjectMap>();
            csvReader.Context.RegisterClassMap<RecordMap>();

            ParsingStage parsingStage = ParsingStage.None;


            while (await csvReader.ReadAsync())
            {
                string? firstCol = csvReader.GetField(0); // get first column

                if (firstCol == null)
                {
                    break;
                }

                firstCol = firstCol.Trim();

                if (firstCol[0] == '[' && firstCol.Last() == ']')
                {
                    parsingStage = firstCol switch
                    {
                        "[currency]" => ParsingStage.Currency,
                        "[objects]" => ParsingStage.Objects,
                        "[records]" => ParsingStage.Records,
                        _ => throw new ArgumentOutOfRangeException("firstCol", 
                            $"unknown symbol {firstCol}")
                    };
                    continue;
                }

                switch (parsingStage)
                {
                    case ParsingStage.Currency:
                        var currency = csvReader.GetRecord<Currency>();
                        result.Currencies.Add(currency);
                        break;
                    case ParsingStage.Objects:
                        var dObject = csvReader.GetRecord<DrebedengiObject>();
                        result.Objects.Add(dObject);
                        break;
                    case ParsingStage.Records:
                        var @record = csvReader.GetRecord<Record>();
                        result.Records.Add(@record);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("parsingStage",
                            $"unknown parsing stage");

                }
            }

            return result;
        }
    }
}