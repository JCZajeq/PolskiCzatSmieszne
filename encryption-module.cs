using System;
using System.IO;
using System.Security.Cryptography; //kryptografia

namespace EncryptionModule
{
    class AesEncrypt
    {
        public static void Main()
        {
            string original = Console.ReadLine(); //Wczytaj wiadomość, która zostanie zaszyfrowana.
            
            using Aes myAes = Aes.Create();
            byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV); //funkcja szyfrująca
        }
        /*argumenty funkcji:
        plainText = wiadomość podana przez użytkownika
        Key = wygenerowany klucz
        IV = wygenerowany wektor inicjujący
        */
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            //Jeśli dowolna zmienna jest pusta wyrzuć nowy wyjątek.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create()) // Nowa instancja klasy Aes.
            {
                aesAlg.Key = Key; //Przypisanie klucza
                aesAlg.IV = IV; //Przypisanie wektora inicjującego

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                
                //utwórz strumienie używane do szyfrowania
                using MemoryStream msEncrypt = new MemoryStream();
                using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Zapisz zaszyfrowaną wiadomość do strumienia.
                    swEncrypt.Write(plainText);
                }
                //Zapisz zaszyfrowaną wiadomość w tablicy encrypted. 
                encrypted = msEncrypt.ToArray();
            }

            string format = "X2"; //Format wiadomości, klucza i IV(X2 to liczba heksadecymalna z dwoma cyframi)
            /*
            Przykłady X2:
            FF
            0A
            44
            04
            */
            Console.WriteLine("Key:");
            foreach(byte byteValue in Key) //Wypisz kolejne bajty klucza. Odzielaj je spacjami.
            {
                Console.Write(byteValue.ToString(format) + " ");
            } Console.Write("\n");
            Console.WriteLine("IV:");
            foreach (byte byteValue in IV) //Wypisz kolejne bajty IV. Odzielaj je spacjami.
            {
                Console.Write(byteValue.ToString(format) + " ");
            } Console.Write("\n");
            Console.WriteLine("Encrypted string:");
            foreach (byte byteValue in encrypted) //Wypisz kolejne bajty wiadomości. Odzielaj je spacjami.
            {
                Console.Write(byteValue.ToString(format) + " ");
            } Console.Write("\n");
            
            //Linia kodu wynikająca z tego, że funkcja encryptStringFromBytes_AES jest typu byte[].
            //Nie powinno jej tu być, ale jestem zbyt leniwy, żeby zmienić przykład z dokumentacji Microsoftu
            return encrypted;
        }
    }
}
