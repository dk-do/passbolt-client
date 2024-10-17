using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Passbolt.Models
{
 

    public partial class LoginRequest
    {
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }

        public LoginRequest(Guid userId, string challenge)
        {
            UserId = userId;
            Challenge = challenge;
            
        }
    }

    

    public partial class LoginRequest
    {
        public static LoginRequest FromJson(string json) => JsonConvert.DeserializeObject<LoginRequest>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LoginRequest self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}

