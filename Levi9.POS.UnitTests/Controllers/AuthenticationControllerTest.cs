using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.Helpers;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Request.ClientRequests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private Mock<IClientService> _clientServiceMock;
        private JwtOptions _jwtOptions;
        [SetUp]
        public void SetUp()
        {
            _clientServiceMock = new Mock<IClientService>();
            _jwtOptions = new JwtOptions
            {
                SigningKey = "mysecret",
                Issuer = "myissuer",
                Audience = "myaudience",
                ExpirationSeconds = 30
            };
        }
        [Test]
        public async Task ClientAuthentication_WithValidClientLogin_ReturnsJwtToken()
        {
            var clientLogin = new ClientLogin
            {
                Email = "john.doe@example.com",
                Password = "mysecretpassword"
            };
            var clientDto = new ClientDto
            {
                Email = clientLogin.Email,
                Password = "hashedpassword",
                Salt = "salt"
            };
            _clientServiceMock.Setup(x => x.GetClientByEmail(clientLogin.Email)).ReturnsAsync(clientDto);
            var controller = new AuthenticationController(_clientServiceMock.Object, _jwtOptions);

            var result = await controller.ClientAuthentication(clientLogin);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task ClientAuthentication_WithInvalidEmail_ReturnsBadRequest()
        {
            var clientLogin = new ClientLogin
            {
                Email = "john.doe@example.com",
                Password = "mysecretpassword"
            };
            _clientServiceMock.Setup(x => x.GetClientByEmail(clientLogin.Email)).ReturnsAsync((ClientDto)null);
            var controller = new AuthenticationController(_clientServiceMock.Object, _jwtOptions);

            var result = await controller.ClientAuthentication(clientLogin) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("Email not valid"));
        }

        [Test]
        public async Task ClientAuthentication_WithInvalidPassword_ReturnsBadRequest()
        {
            var clientLogin = new ClientLogin
            {
                Email = "john.doe@example.com",
                Password = "mysecretpassword"
            };
            var clientDto = new ClientDto
            {
                Email = clientLogin.Email,
                Password = "hashedpassword",
                Salt = "salt"
            };
            _clientServiceMock.Setup(x => x.GetClientByEmail(clientLogin.Email)).ReturnsAsync(clientDto);
            var controller = new AuthenticationController(_clientServiceMock.Object, _jwtOptions);

            var result = await controller.ClientAuthentication(clientLogin) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo("Bad Request - wrong password"));
        }
    }

}
