using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Request;
using Levi9.POS.WebApi.Response;
using Moq;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.ControllerTests
{
    [TestFixture]
    public class ClientControllerTests
    {
        private Mock<IClientService> _clientServiceMock;
        private Mock<IMapper> _mapperMock;
        private ClientController _clientController;

        [SetUp]
        public void Setup()
        {
            _clientServiceMock = new Mock<IClientService>();
            _mapperMock = new Mock<IMapper>();
            _clientController = new ClientController(_clientServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task AddClient_ValidRequest_ShouldReturnOkWithMappedResponse()
        {
            // Arrange
            var clientRequest = new ClientRequest
            {
                Name = "marko",
                Address = "address",
                Phone = "+3851112212",
                Password = "secret",
                Email = "marko@example.com"
            };
            var addClientDto = new AddClientDto
            {
                Id = 1,
                GlobalId = new Guid(),
                Name = "marko",
                Address = "address",
                Phone = "+3851112212",
                Email = "marko@example.com",
                Salt = "salt"
            };
            var clientResponse = new ClientResponse
            {
                Id = 1,
                GlobalId = new Guid(),
                Name = "marko",
                Address = "address",
                Phone = "+3851112212",
                Email = "marko@example.com",
                LastUpdate = DateTime.Now.ToFileTimeUtc().ToString()
            };
            _mapperMock.Setup(m => m.Map<AddClientDto>(clientRequest)).Returns(addClientDto);
            _clientServiceMock.Setup(c => c.AddClient(addClientDto)).ReturnsAsync(addClientDto);
            _mapperMock.Setup(m => m.Map<ClientResponse>(addClientDto)).Returns(clientResponse);

            // Act
            var result = await _clientController.AddClient(clientRequest);

            // Assert
            _mapperMock.Verify(m => m.Map<AddClientDto>(clientRequest), Times.Once);
            _clientServiceMock.Verify(c => c.AddClient(addClientDto), Times.Once);
            _mapperMock.Verify(m => m.Map<ClientResponse>(addClientDto), Times.Once);
        }
    }

}
