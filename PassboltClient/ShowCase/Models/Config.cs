using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowCase.Models
{
    public class Config
    {
        public string BaseUri { get; set; } 
        public string UserId { get; set; }
        public string Password { get; set; }
        public string PrivateKeyPath { get; set; }
    }
}
