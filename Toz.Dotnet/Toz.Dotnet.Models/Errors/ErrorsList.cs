using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Toz.Dotnet.Models.Errors
{
    public class ErrorsList
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
