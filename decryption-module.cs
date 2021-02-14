using System;
using System.IO;
using System.Security.Cryptography; //kryptografia
using System.Collections.Generic; //queue

namespace Aes_Example
{
    class AesExample
    {
        public static void Main()
        {
            
            Queue<byte> MessageBytes = new Queue<byte>(); //kolejka dla kolejnych podanych bajtów wiadomości
            Queue<byte> KeyBytes = new Queue<byte>(); //kolejka dla kolejnych podanych bajtów klucza
            Queue<byte> IVBytes = new Queue<byte>(); //kolejka dla kolejnych podanych bajtów IV

            Console.Write("Otrzymana wiadomość: "); 
            string message = Console.ReadLine(); //Wczytaj zaszyfrowaną wiadmość.
            string[] split = message.Split(' '); //Spacja = separator kolejnych liczb heksadecymalnych. Kolejne liczby zapisz do string[] split.
            foreach (string hex in split) //Każdą liczbę w string[] split zapisz do zmiennej hex.
            {
                byte value = Convert.ToByte(hex, 16); //Byte value równy jest konwersji hex na byte.
                MessageBytes.Enqueue(value); //Dodaj byte value do kolejki bajtów wiadomości.
            }

            Console.Write("Otrzymany klucz: ");
            string SKey = Console.ReadLine(); //Wczytaj klucz. 
            string[] Keysplit = SKey.Split(' '); //Jak wyżej(patrz 17-24).
            foreach (string hex in Keysplit)
            {
                byte value = Convert.ToByte(hex, 16);
                KeyBytes.Enqueue(value);
            }

            Console.Write("Otrzymany wektor inicjujący(IV): ");
            string SIV = Console.ReadLine(); //Wczytaj IV.
            string[] IVsplit = SIV.Split(' '); //Jak wyżej(patrz 17-24).
            foreach (string hex in IVsplit)
            {
                byte value = Convert.ToByte(hex, 16);
                IVBytes.Enqueue(value);
            }
            
            byte[] encrypted = MessageBytes.ToArray(); //Dodaj zawartość kolejki bajtów wiadomości do tablicy. 
            byte[] Key = KeyBytes.ToArray(); //Dodaj zawartość kolejki bajtów klucza do tablicy. 
            byte[] IV = IVBytes.ToArray(); //Dodaj zawartość kolejki bajtów IV do tablicy. 

            using Aes myAes = Aes.Create(); 
            DecryptStringFromBytes_Aes(encrypted, Key, IV);  //funkcja deszyfrująca
        }
        /*argumenty funkcji:
        cipherText = zaszyfrowana wiadomość
        key = klucz
        iv = wektor inicjujący
        */
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            //Jeśli dowolny argument jest pusty wyrzuć nowy wyjątkek.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            
            string plaintext = null; //Zadeklaruj zmienną, która będzie przechowywać odszyfrowaną wiadomość.

            using (Aes aesAlg = Aes.Create()) //Nowa instancja klasy Aes
            {
                aesAlg.Key = Key; //Przypisz klucz.
                aesAlg.IV = IV; //Przypisz IV.

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                
                //Utwórz strumienie używane do deszyfracji
                using MemoryStream msDecrypt = new MemoryStream(cipherText);
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new StreamReader(csDecrypt);
                
                plaintext = srDecrypt.ReadToEnd(); //Odczytaj odszyfrowaną wiadomość i umieść ją w zmiennej.
            }
            Console.WriteLine("Odszyfrowana wiadomość: {0}", plaintext); //Wypisz odszyfrowaną wiadomość.
           
            //Linia kodu wynikająca z tego, że funkcja DecryptStringFromBytes_AES jest typu string.
            //Nie powinno jej tu być, ale jestem zbyt leniwy, żeby zmienić przykład z dokumentacji Microsoftu.
            return plaintext; 
        }
    }
}
