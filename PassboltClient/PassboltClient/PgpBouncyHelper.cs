using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;
using System.IO;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Utilities.Zlib;

public static class PgpBouncyHelper
{
    public static byte[] SignMessage(string message, Stream privateKeyStream, string passphrase)
    {
        PgpSecretKeyRingBundle secretKeyRingBundle = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(privateKeyStream));
        PgpSecretKey secretKey = GetFirstSecretKey(secretKeyRingBundle);
        PgpPrivateKey privateKey = secretKey.ExtractPrivateKey(passphrase.ToCharArray());

        MemoryStream outputStream = new MemoryStream();
        PgpSignatureGenerator signatureGenerator = new PgpSignatureGenerator(secretKey.PublicKey.Algorithm, HashAlgorithmTag.Sha256);
        signatureGenerator.InitSign(PgpSignature.BinaryDocument, privateKey);

        using (Stream literalOut = new ArmoredOutputStream(outputStream))
        {
            PgpLiteralDataGenerator literalDataGenerator = new PgpLiteralDataGenerator();
            using (Stream literalDataOut = literalDataGenerator.Open(literalOut, PgpLiteralData.Binary, "filename", message.Length, DateTime.Now))
            {
                byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
                literalDataOut.Write(messageBytes, 0, messageBytes.Length);
            }

            signatureGenerator.Generate().Encode(literalOut);
        }

        return outputStream.ToArray();
    }

    private static PgpSecretKey GetFirstSecretKey(PgpSecretKeyRingBundle secretKeyRingBundle)
    {
        foreach (PgpSecretKeyRing keyRing in secretKeyRingBundle.GetKeyRings())
        {
            foreach (PgpSecretKey key in keyRing.GetSecretKeys())
            {
                if (key.IsSigningKey)
                {
                    return key;
                }
            }
        }

        throw new ArgumentException("Can't find signing key in key ring.");
    }

    public static byte[] EncryptMessage(byte[] signedMessage, Stream publicKeyStream)
    {
        MemoryStream outputStream = new MemoryStream();
        
        PgpPublicKeyRingBundle publicKeyRingBundle = new PgpPublicKeyRingBundle(PgpUtilities.GetDecoderStream(publicKeyStream));
        PgpPublicKey publicKey = GetFirstPublicKey(publicKeyRingBundle);

        PgpEncryptedDataGenerator encryptedDataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Aes256, true);
        encryptedDataGenerator.AddMethod(publicKey);

        using (Stream encryptedOut = new ArmoredOutputStream(outputStream))
        {
            using (Stream pgpOut = encryptedDataGenerator.Open(encryptedOut, signedMessage.Length))
            {
                pgpOut.Write(signedMessage, 0, signedMessage.Length);
            }
        }

        return outputStream.ToArray();
    }

    private static PgpPublicKey GetFirstPublicKey(PgpPublicKeyRingBundle publicKeyRingBundle)
    {
        foreach (PgpPublicKeyRing keyRing in publicKeyRingBundle.GetKeyRings())
        {
            foreach (PgpPublicKey key in keyRing.GetPublicKeys())
            {
                if (key.IsEncryptionKey)
                {
                    return key;
                }
            }
        }

        throw new ArgumentException("Can't find encryption key in key ring.");
    }

    public static string ConvertToAsciiArmored(byte[] pgpMessage)
    {
        byte[] armoredBytes;

        using (MemoryStream inputStream = new MemoryStream(pgpMessage))
        using (MemoryStream outputStream = new MemoryStream())
        {
            using (ArmoredOutputStream armoredOut = new ArmoredOutputStream(outputStream))
            {
                Streams.PipeAll(inputStream, armoredOut);
                armoredOut.Close();
            }
            
            armoredBytes = outputStream.ToArray();
        }
        
        string armoredText = Encoding.UTF8.GetString(armoredBytes);

        string modifiedArmoredText = armoredText.Replace("\r\n", "\n");

        return modifiedArmoredText;
    }

}
