﻿using Newtonsoft.Json;

namespace Toz.Dotnet.Models
{
    public class ActivationMessage
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }
}
