using Levi9.POS.Domain.Helpers;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.Helpers
{
    [TestFixture]
    public class AuthenticationHelperTests
    {
        [Test]
        public void Validate_ReturnsTrueForValidPassword()
        {
            string passwordHash = AuthenticationHelper.HashPassword("myPassword", "mySalt");
            string salt = "mySalt";
            string password = "myPassword";

            bool isValid = AuthenticationHelper.Validate(passwordHash, salt, password);

            Assert.IsTrue(isValid);
        }
        [Test]
        public void Validate_ReturnsFalseForInvalidPassword()
        {
            string passwordHash = AuthenticationHelper.HashPassword("myPassword", "mySalt");
            string salt = "mySalt";
            string password = "wrongPassword";

            bool isValid = AuthenticationHelper.Validate(passwordHash, salt, password);

            Assert.IsFalse(isValid);
        }
    }
}
