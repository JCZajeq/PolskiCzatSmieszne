using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Aes_Example
{
    class AesExample
    {
        public static void Main()
        {

            Queue<byte> MessageBytes = new Queue<byte>();
            Queue<byte> KeyBytes = new Queue<byte>();
            Queue<byte> IVBytes = new Queue<byte>();

            Console.Write("Otrzymana wiadomość: ");
            string message = Console.ReadLine();
            string[] split = message.Split(' ');
            foreach (string hex in split)
            {
                byte value = Convert.ToByte(hex, 16);
                MessageBytes.Enqueue(value);
            }

            Console.Write("Otrzymany klucz: ");
            string SKey = Console.ReadLine();
            string[] Keysplit = SKey.Split(' ');
            foreach (string hex in Keysplit)
            {
                byte value = Convert.ToByte(hex, 16);
                KeyBytes.Enqueue(value);
            }

            Console.Write("Otrzymany wektor inicjujący(IV): ");
            string SIV = Console.ReadLine();
            string[] IVsplit = SIV.Split(' ');
            foreach (string hex in IVsplit)
            {
                byte value = Convert.ToByte(hex, 16);
                IVBytes.Enqueue(value);
            }
            byte[] encrypted = MessageBytes.ToArray();
            byte[] Key = KeyBytes.ToArray();
            byte[] IV = IVBytes.ToArray();

            using Aes myAes = Aes.Create();
            DecryptStringFromBytes_Aes(encrypted, Key, IV);
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new MemoryStream(cipherText);
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new StreamReader(csDecrypt);

                plaintext = srDecrypt.ReadToEnd();
            }
            Console.WriteLine("Odszyfrowana wiadomość: {0}", plaintext);
            return plaintext;
        }
    }
}
