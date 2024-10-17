using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Passbolt.Models
{
    public partial class ServerKeyResponse
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("keydata")]
        public string Keydata { get; set; }
    }

    public partial class Header
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("servertime")]
        public long Servertime { get; set; }

        [JsonProperty("action")]
        public Guid Action { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }
    }

  
    
}
