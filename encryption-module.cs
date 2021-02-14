using System;
using System.IO;
using System.Security.Cryptography;

namespace EncryptionModule
{
    class AesEncrypt
    {
        public static void Main()
        {
            string original = Console.ReadLine();
            using Aes myAes = Aes.Create();
            byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);
        }
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new MemoryStream();
                using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }

            string format = "X2";
            Console.WriteLine("Key:");
            foreach(byte byteValue in Key)
            {
                Console.Write(byteValue.ToString(format) + " ");
            } Console.Write("\n");
            Console.WriteLine("IV:");
            foreach (byte byteValue in IV)
            {
                Console.Write(byteValue.ToString(format) + " ");
            } Console.Write("\n");
            Console.WriteLine("Encrypted string:");
            foreach (byte byteValue in encrypted)
            {
                Console.Write(byteValue.ToString(format) + " ");
            } Console.Write("\n");
            return encrypted;
        }
    }
}
