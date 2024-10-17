using Newtonsoft.Json;
using Passbolt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbolt
{
    public class PgpClient
    {
        public async static Task<string> SignAndEncryptLoginChallenge(string passboltSrvBaseUri, string privateKeyFile, string passphrase)
        {
            PassClient client = new PassClient();

            var serverKey = await client.GetServerPublicKey(passboltSrvBaseUri);
            var publicKey = serverKey.Body.Keydata;
            byte[] byteArray = Encoding.UTF8.GetBytes(publicKey);


            JwtChallenge jwt = new JwtChallenge(passboltSrvBaseUri, serverKey.Header.Id.ToString());
            string message = JsonConvert.SerializeObject(jwt);
            using (FileStream privateKeyStream = File.OpenRead(privateKeyFile))
            using (MemoryStream publicKeyStream = new MemoryStream(byteArray))
            {
                // Signiere die Nachricht
                byte[] signedMessage = PgpBouncyHelper.SignMessage(message, privateKeyStream, passphrase);

                // Verschlüssele die signierte Nachricht
                byte[] encryptedMessage = PgpBouncyHelper.EncryptMessage(signedMessage, publicKeyStream);

                // Konvertiere die verschlüsselte Nachricht in ASCII-armored Format
                string armoredMessage = PgpBouncyHelper.ConvertToAsciiArmored(encryptedMessage);


                return armoredMessage;

            }

            //var signedMessage = await PgpCoreHelper.SignMessageAsync(new FileInfo(privateKeyFile), passphrase, message);
            //var encryptedMessage = await PgpCoreHelper.EncryptMessageAsync(publicKey, signedMessage);

            //var res = await PgpCoreHelper.EncryptAndSignMessageAsync(publicKey, new FileInfo(privateKeyFile), passphrase, message);
            
           
        }
    }
}
