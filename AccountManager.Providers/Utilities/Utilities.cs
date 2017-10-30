using System.Security.Cryptography;
using System.Text;

namespace AccountManager.Providers.Utilities
{
    public static class Utilities
    {
        public static string GenerateMd5(this string value)
        {
            byte[] bytes;
            using (MD5 md5 = MD5.Create())
            {
                md5.Initialize();
                md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                bytes = md5.Hash;

                var result = new StringBuilder(bytes.Length * 2);

                for (int i = 0; i < bytes.Length; i++)
                    result.Append(bytes[i].ToString("x2"));

                return result.ToString();
            }
        }
    }
}
