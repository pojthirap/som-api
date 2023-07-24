using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyFirstAzureWebApp.Utils
{
    public static class GenerateKey
    {

        public static string generateFileIdKey(string str)
        {
            string base64Str =  Base64Utils.Base64StringEncode(str);
            string val = Encrypt(base64Str);
            return val;
        }


        public static string getFileId(string key)
        {
            string val = Decrypt(key);
            string str = Base64Utils.Base64StringDecode(val);
            return str;
        }



        private static byte[] Key = Encoding.ASCII.GetBytes(@"qwr{@^h`h&_`50/ja9!'dcmh3!uw<&=?");
        //private static byte[] IV = Encoding.ASCII.GetBytes(@"9/\~V).A,lY&=t2b");

        static Dictionary<string, byte[]> dictionaryIV = new Dictionary<string, byte[]>{
            {"10", Encoding.ASCII.GetBytes(@"9/\~V).A,gC&=a1u")},
            {"11", Encoding.ASCII.GetBytes(@"8/\~W).B,hD&=b2t")},
            {"12", Encoding.ASCII.GetBytes(@"7/\~X).C,iE&=c3s")},
            {"13", Encoding.ASCII.GetBytes(@"6/\~Y).D,jF&=d4r")},
            {"14", Encoding.ASCII.GetBytes(@"5/\~Z).E,kG&=e5q")},
            {"15", Encoding.ASCII.GetBytes(@"4/\~A).F,lH&=f6p")},
            {"16", Encoding.ASCII.GetBytes(@"3/\~B).G,mI&=g7o")},
            {"17", Encoding.ASCII.GetBytes(@"2/\~C).H,nJ&=h8n")},
            {"18", Encoding.ASCII.GetBytes(@"1/\~D).I,oK&=i9m")},
            {"19", Encoding.ASCII.GetBytes(@"0/\~E).J,pL&=j0l")},
            {"20", Encoding.ASCII.GetBytes(@"1/\~F).K,qM&=k1k")},
            {"21", Encoding.ASCII.GetBytes(@"2/\~G).L,rN&=l2j")},
            {"22", Encoding.ASCII.GetBytes(@"3/\~H).M,sO&=m3i")},
            {"23", Encoding.ASCII.GetBytes(@"4/\~I).N,tP&=n4h")},
            {"24", Encoding.ASCII.GetBytes(@"5/\~J).O,uQ&=o5g")},
            {"25", Encoding.ASCII.GetBytes(@"6/\~K).P,vR&=p6f")},
            {"26", Encoding.ASCII.GetBytes(@"7/\~L).Q,wS&=q7e")},
            {"27", Encoding.ASCII.GetBytes(@"8/\~M).R,xT&=r8d")},
            {"28", Encoding.ASCII.GetBytes(@"9/\~N).S,yU&=s9c")},
            {"29", Encoding.ASCII.GetBytes(@"0/\~O).T,zV&=t0b")},
            {"30", Encoding.ASCII.GetBytes(@"1/\~P).U,aW&=u1a")},
            {"31", Encoding.ASCII.GetBytes(@"2/\~Q).V,bx&=v2b")},
            {"32", Encoding.ASCII.GetBytes(@"3/\~R).W,cy&=w3c")},
            {"33", Encoding.ASCII.GetBytes(@"4/\~S).X,dz&=x4d")},
            {"34", Encoding.ASCII.GetBytes(@"5/\~T).Y,ea&=y5e")},
            {"35", Encoding.ASCII.GetBytes(@"6/\~U).Z,fb&=z6f")},
            {"36", Encoding.ASCII.GetBytes(@"7/\~V).A,gc&=a7g")},
            {"37", Encoding.ASCII.GetBytes(@"8/\~W).B,hd&=b8h")},
            {"38", Encoding.ASCII.GetBytes(@"9/\~X).C,ie&=c9i")},
            {"39", Encoding.ASCII.GetBytes(@"0/\~Y).D,jf&=d0j")},
            {"40", Encoding.ASCII.GetBytes(@"1/\~Z).E,kg&=e1k")},
            {"41", Encoding.ASCII.GetBytes(@"2/\~A).F,lh&=f2l")},
            {"42", Encoding.ASCII.GetBytes(@"3/\~B).G,mi&=g3m")},
            {"43", Encoding.ASCII.GetBytes(@"4/\~C).H,nj&=h4n")},
            {"44", Encoding.ASCII.GetBytes(@"5/\~D).I,ok&=i5o")},
            {"45", Encoding.ASCII.GetBytes(@"6/\~E).J,pl&=j6p")},
            {"46", Encoding.ASCII.GetBytes(@"7/\~F).K,qm&=k7q")},
            {"47", Encoding.ASCII.GetBytes(@"8/\~G).L,rn&=l8r")},
            {"48", Encoding.ASCII.GetBytes(@"9/\~H).M,so&=m9s")},
            {"49", Encoding.ASCII.GetBytes(@"0/\~I).N,tp&=n0t")},
            {"50", Encoding.ASCII.GetBytes(@"1/\~J).O,uq&=o1u")},
            {"51", Encoding.ASCII.GetBytes(@"2/\~K).P,vr&=p2v")},
            {"52", Encoding.ASCII.GetBytes(@"3/\~L).Q,ws&=q3w")},
            {"53", Encoding.ASCII.GetBytes(@"4/\~M).R,xt&=r4x")},
            {"54", Encoding.ASCII.GetBytes(@"5/\~N).S,yu&=s5y")},
            {"55", Encoding.ASCII.GetBytes(@"6/\~O).T,zv&=t6z")},
            {"56", Encoding.ASCII.GetBytes(@"7/\~P).U,aw&=u7a")},
            {"57", Encoding.ASCII.GetBytes(@"8/\~Q).V,bx&=v8b")},
            {"58", Encoding.ASCII.GetBytes(@"9/\~R).W,cy&=w1c")},
            {"59", Encoding.ASCII.GetBytes(@"0/\~S).X,dz&=x9d")},
            {"60", Encoding.ASCII.GetBytes(@"1/\~T).Y,ea&=y0e")}
        };

        public static string Encrypt(string original)
        {

            Random rnd = new Random();
            int ivIndex = rnd.Next(10, 61); // creates a number between 10 and 60
            {
                /*for(int i = 0; i < 1000; i++)
                {
                    rnd = new Random();
                    ivIndex = rnd.Next(10, 61); // creates a number between 10 and 60
                    Console.WriteLine(ivIndex);
                }*/
                
            }
            

            byte[] IV = dictionaryIV[ivIndex.ToString()];

            string encryptStr;
            using (Aes myAes = Aes.Create())
            {

                // Encrypt the string to an array of bytes.
                encryptStr = EncryptStringToBytes_Aes(original, Key, IV);
                encryptStr = Base64Utils.Base64StringEncode(encryptStr);
                //Display the original data and the decrypted data.
                //Console.WriteLine("Original:   "+ original);
                //Console.WriteLine("Encrypt : "+ encryptStr);
            }
            return encryptStr+ ivIndex;
        }

        public static string Decrypt(string encryptStr)
        {

            int startIndex = encryptStr.Length - 2;
            string ivIndex = encryptStr.Substring(startIndex);

            startIndex = 0;
            int endIndex = encryptStr.Length - 2;
            encryptStr = encryptStr.Substring(startIndex, endIndex);



            byte[] IV = dictionaryIV[ivIndex];

            string decryptStr;
            using (Aes myAes = Aes.Create())
            {
                decryptStr = Base64Utils.Base64StringDecode(encryptStr);
                // Decrypt the bytes to a string.
                decryptStr = DecryptStringFromBytes_Aes(decryptStr, Key, IV);
                //Display the original data and the decrypted data.
                //Console.WriteLine("Original:   "+ encryptStr);
                //Console.WriteLine("Round Trip: "+ decryptStr);
            }
            return decryptStr;
        }




        /*public static void Main()
        {
            string original = "Here is some data to encrypt!";

            string decryptStr = "";
            string encryptStr = "";



            // Create a new instance of the Aes
            // class.  This generates a new key and initialization
            // vector (IV).
            using (Aes myAes = Aes.Create())
            {

                // Encrypt the string to an array of bytes.
                encryptStr = EncryptStringToBytes_Aes(original, Key, IV);
                //Display the original data and the decrypted data.
                Console.WriteLine("Original:   "+ original);
                Console.WriteLine("Encrypt : "+ encryptStr);
            }



            using (Aes myAes = Aes.Create())
            {
                // Decrypt the bytes to a string.
                decryptStr = DecryptStringFromBytes_Aes(encryptStr, Key, IV);
                //Display the original data and the decrypted data.
                Console.WriteLine("Original:   "+ encryptStr);
                Console.WriteLine("Round Trip: "+ decryptStr);
            }

        }
        */

        static string EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            //return encrypted;
            return Convert.ToBase64String(encrypted);
        }

        static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextString);
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }






    }











}
