using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace managerelchenchenvuelve.Recursos
{
    public class EncryptPass : Controller
    {
        public static string Encriptar(string clave)
        {
            if (clave == null)
            {
                throw new ArgumentNullException(nameof(clave), "Ingrese su contraseña");
            }

            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(clave));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString().ToUpper(); ;
        }
    }
}
