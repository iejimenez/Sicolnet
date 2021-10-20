using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sicolnet.Utils
{
    public class Encriptador
    {
        #region Encriptar
        /// <summary>
        /// Método para encriptar un texto plano usando el algoritmo (Rijndael).
        /// Este es el mas simple posible, muchos de los datos necesarios los
        /// definimos como constantes.
        /// </summary>
        /// <param name="textoQueEncriptaremos">Texto a encriptar</param>
        /// <returns>Texto encriptado</returns>
        public static string Encriptar(string textoQueEncriptaremos)
        {
            return Encriptar(textoQueEncriptaremos,
              "pass75dc@SicoLNet", "s@SicoLNet", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
        }
        /// <summary>
        /// Método para encriptar un texto plano usando el algoritmo (Rijndael)
        /// </summary>
        /// <returns>Texto encriptado</returns>
        public static string Encriptar(string textoQueEncriptaremos,
          string passBase, string saltValue, string hashAlgorithm,
          int passwordIterations, string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,
              saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC
            };
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes,
              initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor,
             CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }
        public static string DesEncriptar(string textoQueDesEncriptaremos)
        {
            return DesEncriptar(textoQueDesEncriptaremos.Trim(),
              "pass75dc@SicoLNet", "s@SicoLNet", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
        }


        public static string DesEncriptar(string textoEncriptado,
            string passBase, string saltValue, string hashAlgorithm,
          int passwordIterations, string initVector, int keySize)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,
              saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);


            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            Rijndael RijndaelAlg = Rijndael.Create();

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         RijndaelAlg.CreateDecryptor(keyBytes, initVectorBytes),
                                                         CryptoStreamMode.Read);


            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);


        }
        #endregion

    }
}
