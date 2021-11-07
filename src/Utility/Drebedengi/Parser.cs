using System;
using System.IO;
using System.Threading.Tasks;

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

            ParsingStage parsingStage = ParsingStage.None;

            while (!reader.EndOfStream)
            {
                string? line = await reader.ReadLineAsync();

                if (line == null)
                {
                    break;
                }

                line = line.Trim();

                if (line.StartsWith('['))
                {
                    parsingStage = line switch
                    {
                        "[currency]" => ParsingStage.Currency,
                        "[objects]" => ParsingStage.Objects,
                        "[records]" => ParsingStage.Records,
                        _ => throw new ArgumentOutOfRangeException("line", $"unknown symbol {line}")
                    };
                    continue;
                }

                switch (parsingStage)
                {
                    case ParsingStage.Currency:
                        // todo: parse currencies
                        result.Currencies.Add(currency);
                        break;
                    case ParsingStage.Objects:
                        // todo: parse objects
                        result.Objects.Add(@object);
                        break;
                    case ParsingStage.Records:
                        // todo: parse records
                        result.Records.Add(@record);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("parsingStage",
                            $"unknown parsing stage");

                }
            }
        }
    }
}