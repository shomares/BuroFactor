using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BuroComun.src.security.impl
{
    public class EncriptaRSA: IEncripta
    {
        public X509Certificate2 Certificado { get; set; }

        public string Decrypt(String value) {
            var rsa = Certificado.PrivateKey as RSACryptoServiceProvider;
            var decodedData = rsa.Decrypt(Convert.FromBase64String(value), true);
            return Encoding.UTF8.GetString(decodedData);
        }
        public string Encrypt(String value)
        {
            var rsa = Certificado.PublicKey.Key as RSACryptoServiceProvider;
            var decodedData = rsa.Encrypt(Encoding.UTF8.GetBytes(value), true);
            return Convert.ToBase64String(decodedData);
        }


    }
}
