using System;
using System.Security.Cryptography;
using System.Text;

namespace FMASolutionsCore.DataServices.CryptoHelper
{
    public class CryptoService
    {
        public static string Encrypt(string plainText, string privateKey, string salt)
        {
            AesCryptoServiceProvider encDec = new AesCryptoServiceProvider();
            encDec.BlockSize = 128;
            encDec.KeySize = 256;
            encDec.Key = ASCIIEncoding.ASCII.GetBytes(privateKey.Substring(0,encDec.Key.Length));
            encDec.IV = ASCIIEncoding.ASCII.GetBytes(salt.Substring(encDec.IV.Length));
            encDec.Padding = PaddingMode.PKCS7;
            encDec.Mode = CipherMode.CBC;

            using (ICryptoTransform icrypt = encDec.CreateEncryptor(encDec.Key, encDec.IV))
            {
                return Convert.ToBase64String(
                    inArray: icrypt.TransformFinalBlock(
                        inputBuffer: ASCIIEncoding.ASCII.GetBytes(plainText)
                        , inputOffset: 0
                        , inputCount: ASCIIEncoding.ASCII.GetBytes(plainText).Length
                    )
                );
            }
        }

        public static string Decrypt(string encryptedText, string privateKey, string salt)
        {
            AesCryptoServiceProvider encDec = new AesCryptoServiceProvider();
            encDec.BlockSize = 128;
            encDec.KeySize = 256;
            encDec.Key = ASCIIEncoding.ASCII.GetBytes(privateKey);
            encDec.IV = ASCIIEncoding.ASCII.GetBytes(salt);
            encDec.Padding = PaddingMode.PKCS7;
            encDec.Mode = CipherMode.CBC;

            using (ICryptoTransform icrypt = encDec.CreateDecryptor(encDec.Key, encDec.IV))
            {
                return ASCIIEncoding.ASCII.GetString(
                    bytes: icrypt.TransformFinalBlock(
                        inputBuffer: Convert.FromBase64String(encryptedText)
                        , inputOffset: 0
                        , inputCount: Convert.FromBase64String(encryptedText).Length
                    )
                );
            }


        }
    }
}
