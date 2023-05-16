using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Levi9.POS.IntegrationTests.Fixtures
{
    public static class ProductFixture
    {
        public static string GenerateJwt()
        {
            var securityKey = Encoding.UTF8.GetBytes("some-signing-key-here");
            var symetricKey = new SymmetricSecurityKey(securityKey);
            var signingCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                                            issuer: "http://localhost:5067",
                                            audience: "http://localhost:5067",
                                            expires: DateTime.Now.Add(new TimeSpan(86400)),
                                            signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
