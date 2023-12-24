using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Models
{

    public class CandleData
    {
        public DateTime Timestamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }

        [JsonConstructor]

        public CandleData(JsonDocument jsonData)
        {

            var values = jsonData.RootElement.EnumerateArray();

            if (values.Count() == 6)
            {
                var enumerator = values.GetEnumerator();

                if (enumerator.MoveNext() && enumerator.Current.ValueKind == JsonValueKind.String && DateTime.TryParseExact(enumerator.Current.GetString(), "yyyy-MM-ddTHH:mm:sszzz", null, System.Globalization.DateTimeStyles.None, out DateTime timestamp))
                {
                    Timestamp = timestamp;
                }
                else
                {
                    Console.WriteLine("Error parsing timestamp.");
                }

                if (enumerator.MoveNext() && enumerator.Current.TryGetDouble(out double open))
                {
                    Open = open;
                }
                else
                {
                    Console.WriteLine("Error parsing open value.");
                }

                if (enumerator.MoveNext() && enumerator.Current.TryGetDouble(out double high))
                {
                    High = high;
                }
                else
                {
                    Console.WriteLine("Error parsing high value.");
                }

                if (enumerator.MoveNext() && enumerator.Current.TryGetDouble(out double low))
                {
                    Low = low;
                }
                else
                {
                    Console.WriteLine("Error parsing low value.");
                }

                if (enumerator.MoveNext() && enumerator.Current.TryGetDouble(out double close))
                {
                    Close = close;
                }
                else
                {
                    Console.WriteLine("Error parsing close value.");
                }

                if (enumerator.MoveNext() && enumerator.Current.TryGetInt64(out long volume))
                {
                    Volume = volume;
                }
                else
                {
                    Console.WriteLine("Error parsing volume value.");
                }
            }
            else
            {
                Console.WriteLine("Invalid data format");
            }

        }

    }

}
