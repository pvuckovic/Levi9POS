using Levi9.POS.Domain.Common;
using System.Security.Cryptography;
using System.Text;

namespace Levi9.POS.Domain.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        public string HashPassword(string password, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public string GenerateRandomSalt(int length = 32)
        {
            string randomString = Convert.ToBase64String(RandomNumberGenerator.GetBytes(length));
            return new string(randomString);
        }
    }
}
