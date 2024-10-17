
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Prng;
using Passbolt;
using Passbolt.Models;
using ShowCase.Models;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ShowCase
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var cfg = ReadConfig();
                var challengeMessage = await PgpClient.SignAndEncryptLoginChallenge(cfg.BaseUri, cfg.PrivateKeyPath, cfg.Password);
                await Passbolt(challengeMessage, new Guid(cfg.UserId), cfg.BaseUri);
            }
            catch (Exception)
            {

                throw;
            }

        }

        static Config ReadConfig()
        {
            var cfgDir = Path.Combine(AppContext.BaseDirectory, "cfg");
            var cfgFile = Path.Combine(cfgDir, "config.json");
            if (!Directory.Exists($"{cfgDir}"))
            {
                Directory.CreateDirectory(cfgDir);
            }

            if (File.Exists(cfgFile))
            {
                var cfgString = File.ReadAllText(cfgFile);
                Config showCaseConfig = JsonConvert.DeserializeObject<Config>(cfgString);
                return showCaseConfig;
            }
            else
            {
                Config config = new Config();
                Console.WriteLine("No config exists, starting with a new config!");
                Console.Write("User Id: ");
                config.UserId = Console.ReadLine();
                Console.WriteLine("===========================");
                Console.Write("Password: ");
                config.Password = Console.ReadLine();
                Console.WriteLine("===========================");
                Console.Write("Base Uri: ");
                config.BaseUri = Console.ReadLine();
                Console.WriteLine("===========================");
                Console.Write("Path and filename to private key: ");
                config.PrivateKeyPath = Console.ReadLine();
                var cfgString = JsonConvert.SerializeObject(config, Formatting.Indented);
                using (StreamWriter sw = new StreamWriter(cfgFile)) { sw.WriteLine(cfgString); }
                return config;



            }
        }

        static async Task Passbolt(string challenge, Guid userId, string domain)
        {
            try
            {
                                
                LoginRequest request = new LoginRequest(userId, challenge);
                PassClient client = new PassClient();
                await client.Login(domain, request);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        
    }
}
