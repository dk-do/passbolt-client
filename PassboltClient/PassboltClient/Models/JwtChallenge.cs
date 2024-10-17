using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbolt.Models
{
    public class JwtChallenge
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("verify_token")]
        public string VerifyToken { get; set; }

        [JsonProperty("verify_token_expiry")]
        public long VerifyTokenExpiry { get; set; }

        public JwtChallenge(string domain, string uuid)
        {
            Version = "1.0.0";
            Domain = domain;
            VerifyToken = uuid;
            VerifyTokenExpiry = GetUnixTimestamp(DateTime.Now.AddMinutes(2));

        }

        private long GetUnixTimestamp(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

    }
}
