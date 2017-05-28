using Newtonsoft.Json;
using System;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models.Images
{
    public class Photo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createDate")]
        [JsonConverter(typeof(JsonDateTimeConventer))]
        public DateTime CreateDate { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("fileUrl")]
        public string FileUrl { get; set; }
    }
}
