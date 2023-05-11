using AutoMapper;
using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Request;
using Levi9.POS.WebApi.Request.ClientRequests;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.Controllers
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
            var addClientDto = new ClientDto
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
            _mapperMock.Setup(m => m.Map<ClientDto>(clientRequest)).Returns(addClientDto);
            _clientServiceMock.Setup(c => c.AddClient(addClientDto)).ReturnsAsync(addClientDto);
            _mapperMock.Setup(m => m.Map<ClientResponse>(addClientDto)).Returns(clientResponse);

            // Act
            var result = await _clientController.AddClient(clientRequest);

            // Assert
            _mapperMock.Verify(m => m.Map<ClientDto>(clientRequest), Times.Once);
            _clientServiceMock.Verify(c => c.AddClient(addClientDto), Times.Once);
            _mapperMock.Verify(m => m.Map<ClientResponse>(addClientDto), Times.Once);
        }
        [Test]
        public async Task GetClientById_WithValidId_ReturnsOkResultWithClientResponse()
        {
            int clientId = 1;
            var clientDto = new ClientDto { Id = clientId, Name = "John Doe", Email = "johndoe@example.com" };
            _clientServiceMock.Setup(x => x.GetClientById(clientId)).ReturnsAsync(clientDto);

            var result = await _clientController.GetClientById(clientId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var clientResponse = (ClientResponse)((OkObjectResult)result).Value;
        }
        [Test]
        public async Task GetClientById_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int clientId = -1;

            // Act
            var result = await _clientController.GetClientById(clientId);

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value, Is.EqualTo("Id must be a positive integer"));
        }
        [Test]
        public async Task GetClientByGlobalId_WithExistingClient_ReturnsOk()
        {
            Guid globalId = Guid.NewGuid();
            var clientDto = new ClientDto
            {
                GlobalId = globalId,
                Name = "John",
                Email = "john@test.com"
            };
            var clientResponse = new ClientResponse
            {
                GlobalId = globalId,
                Name = "John",
                Email = "john@test.com"
            };
            _clientServiceMock.Setup(cs => cs.GetClientByGlobalId(globalId)).ReturnsAsync(clientDto);
            _mapperMock.Setup(m => m.Map<ClientResponse>(clientDto)).Returns(clientResponse);

            var result = await _clientController.GetClientByGlobalId(globalId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var response = (ClientResponse)okResult.Value;
            Assert.That(response.GlobalId, Is.EqualTo(clientResponse.GlobalId));
            Assert.That(response.Name, Is.EqualTo(clientResponse.Name));
            Assert.That(response.Email, Is.EqualTo(clientResponse.Email));
        }
        [Test]
        public async Task GetClientByGlobalId_WithNonExistingClient_ReturnsNotFound()
        {
            Guid globalId = Guid.NewGuid();
            _clientServiceMock.Setup(cs => cs.GetClientByGlobalId(globalId)).ReturnsAsync((ClientDto)null);

            var result = await _clientController.GetClientByGlobalId(globalId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.That(notFoundResult.Value, Is.EqualTo($"Client with Id {globalId} not found"));
        }

    }

}
