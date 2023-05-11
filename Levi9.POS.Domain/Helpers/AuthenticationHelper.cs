using System.Security.Cryptography;
using System.Text;

namespace Levi9.POS.Domain.Helpers
{
    public static class AuthenticationHelper
    {
        public static string HashPassword(string password, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static string GenerateRandomSalt(int length = 10)
        {
            string randomString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(length));
            return new string(randomString);
        }
        public static bool Validate(string passwordHash, string salt, string password)
        {
            if(HashPassword(password, salt) == passwordHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
