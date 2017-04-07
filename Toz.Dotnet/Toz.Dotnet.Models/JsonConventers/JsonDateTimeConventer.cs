using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace Toz.Dotnet.Models.JsonConventers
{
    public class JsonDateTimeConventer : DateTimeConverterBase
    {
        private const string Format = "dd-MM-yyyy HH:mm:ss";

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            var s = reader.Value.ToString();
            
            DateTime dt = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dt = dt.AddSeconds(Math.Round(Convert.ToDouble(s) / 1000)).ToLocalTime();

            return dt;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime result;
            var s = value.ToString();

            if(DateTime.TryParseExact(s, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) {
                Int64 unixTimestamp = (Int64)(result.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                writer.WriteValue(unixTimestamp);
            }
        }
    }
}