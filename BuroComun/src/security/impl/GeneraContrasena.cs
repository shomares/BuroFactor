using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuroComun.src.security.impl
{
    public class GeneraContrasena : IGeneraContrasena
    {

        public int MaxSaltLength { get; set; }

        private byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        public string SHA256Encrypt(string input)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }


        public string generaAleatorio()
        {
            return Guid.NewGuid().ToString();

        }
        public string generaPassword()
        {
            return Guid.NewGuid().ToString();
        }

        public string generaPassword(String salt, String pass)
        {
            String passNuevo = pass + salt;
            for (int i = 0; i < 100; i++)
            {
                passNuevo = SHA256Encrypt(passNuevo);
            }
            return passNuevo;
        }

        public string generaSal()
        {
            var bytesSalt = GetSalt(MaxSaltLength);
            return Encoding.UTF8.GetString(bytesSalt);
        }
    }
}
