using PgpCore;


namespace Passbolt
{
    public class PgpCoreHelper
    {
        public static async Task<string> EncryptAndSignMessageAsync(string publicKey, FileInfo privateKey, string privateKeyPassword, string message)
        {
            string privateKeyStr = File.ReadAllText(privateKey.FullName);
            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey, privateKeyStr, privateKeyPassword);
            PGP pgp = new PGP(encryptionKeys);           
            string encryptedSignedContent = await pgp.EncryptAndSignAsync(message);
            return encryptedSignedContent
        }
                
        public static async Task<string> EncryptMessageAsync(string publicKey, string message)
        {            
            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);
            PGP pgp = new PGP(encryptionKeys);
            string encryptedContent = await pgp.EncryptAsync(message);
            return encryptedContent;
        }

        public static async Task<string> SignMessageAsync(FileInfo privateKey, string privateKeyPassword, string message)
        {
            string privateKeyStr = File.ReadAllText(privateKey.FullName);
            EncryptionKeys encryptionKeys = new EncryptionKeys(privateKeyStr, privateKeyPassword);
            PGP pgp = new PGP(encryptionKeys);
            string encryptedSignedContent = await pgp.SignAsync(message);
            return encryptedSignedContent;
        }

    }

    
}