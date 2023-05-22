using AutoMapper;
using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.DTOs.ProductDTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Services;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.Services
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<ClientService>> _loggerMock;
        private ClientService _clientService;
        [SetUp]
        public void Setup()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _loggerMock = new Mock<ILogger<ClientService>>();
            _mapperMock = new Mock<IMapper>();
            _clientService = new ClientService(_clientRepositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
        }
        [Test]
        public async Task AddClient_ValidDto_ShouldCallRepositoryAndReturnMappedDto()
        {
            var clientModel = new ClientDto
            {
                Name = "Zlatko",
                Address = "address",
                Email = "zlatko@example.com",
                Password = "R+AKYqbYP0P/E9P1mCH5SjXOGZPbEVk79fNnUydFmWY=",
                Salt = "RZVxPvmCKmwVTA==",
                Phone = "064322222",
                LastUpdate = "634792557112051692"
            };
            var createdClient = new ClientDto
            {
                GlobalId = Guid.NewGuid(),
                Email = clientModel.Email,
                Password = clientModel.Password,
                Salt = clientModel.Salt,
                LastUpdate = DateTime.Now.ToFileTimeUtc().ToString()
            };
            _clientRepositoryMock.Setup(x => x.AddClient(clientModel)).Returns(createdClient);

            var result = await _clientService.AddClient(clientModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo(clientModel.Email));
            Assert.That(result.Name, Is.EqualTo(clientModel.Name));
            Assert.That(result.Phone, Is.EqualTo(clientModel.Phone));
        }
        [Test]
        public async Task GetClientByIdAsync_WithExistingClientId_ReturnsClientDTO()
        {
            int clientId = 1;
            var client = new Client { Id = clientId };
            var clientDto = new ClientDto { Id = clientId };
            _clientRepositoryMock.Setup(x => x.GetClientById(clientId)).ReturnsAsync(client);
            _mapperMock.Setup(x => x.Map<ClientDto>(client)).Returns(clientDto);

            var result = await _clientService.GetClientById(clientId);

            Assert.That(result, Is.EqualTo(clientDto));
        }
        [Test]
        public async Task GetClientByGlobalIdAsync_WithExistingClientId_ReturnsClientDTO()
        {
            Guid globalId = new Guid();
            var client = new Client { GlobalId = globalId };
            var clientDto = new ClientDto { GlobalId = globalId };
            _clientRepositoryMock.Setup(x => x.GetClientByGlobalId(globalId)).ReturnsAsync(client);
            _mapperMock.Setup(x => x.Map<ClientDto>(client)).Returns(clientDto);

            var result = await _clientService.GetClientByGlobalId(globalId);

            Assert.That(result, Is.EqualTo(clientDto));
        }
        [Test]
        public void CheckEmailExist_Should_Return_True_If_Email_Exists()
        {
            var email = "test@example.com";
            _clientRepositoryMock.Setup(x => x.CheckEmailExist(email)).Returns(true);

            var result = _clientService.CheckEmailExist(email);

            Assert.IsTrue(result);
        }
        [Test]
        public void CheckEmailExist_Should_Return_False_If_Email_Does_Not_Exist()
        {
            var email = "test@example.com";
            _clientRepositoryMock.Setup(x => x.CheckEmailExist(email)).Returns(false);

            var result = _clientService.CheckEmailExist(email);

            Assert.IsFalse(result);
        }
        [Test]
        public async Task UpdateClient_Should_Return_Updated_Client()
        {
            var updateClientDto = new UpdateClientDto { Id = 1, Name = "John Doe" };
            var expectedClient = new Client { Id = 1, Name = "John Doe" };
            _mapperMock.Setup(x => x.Map<Client>(updateClientDto)).Returns(expectedClient);
            _clientRepositoryMock.Setup(x => x.UpdateClient(expectedClient)).ReturnsAsync(expectedClient);

            var result = await _clientService.UpdateClient(updateClientDto);

            Assert.That(result, Is.EqualTo(updateClientDto));
        }
        [Test]
        public async Task GetAllClients_ReturnsOkWithMappedList_WhenServiceReturnsNonEmptyList()
        {
            var lastUpdate = "123288706851213387";
            var client1 = new Client { Id = 1, Name = "Client 1" };
            var client2 = new Client { Id = 2, Name = "Client 2" };
            var clients = new List<Client> { client1, client2 };
            var expectedResponse1 = new UpdateClientDto { Id = 1, Name = "Client 1" };
            var expectedResponse2 = new UpdateClientDto { Id = 2, Name = "Client 2" };
            var expectedResponses = new List<UpdateClientDto> { expectedResponse1, expectedResponse2 };

            _clientRepositoryMock.Setup(x => x.GetClientsByLastUpdate(lastUpdate)).ReturnsAsync(clients);
            _mapperMock.Setup(x => x.Map<UpdateClientDto>(client1)).Returns(expectedResponse1);
            _mapperMock.Setup(x => x.Map<UpdateClientDto>(client2)).Returns(expectedResponse2);

            var result = await _clientService.GetClientsByLastUpdate(lastUpdate);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<UpdateClientDto>>());
            Assert.That(result.Count(), Is.EqualTo(expectedResponses.Count));
            CollectionAssert.AreEqual(expectedResponses, result);
        }
        [Test]
        public async Task GetAllClients_ReturnsOkWithEmptyList_WhenServiceReturnsEmptyList()
        {
            var lastUpdate = "833288706851213387";
            var emptyList = Enumerable.Empty<Client>();
            _clientRepositoryMock.Setup(x => x.GetClientsByLastUpdate(lastUpdate)).ReturnsAsync(emptyList);

            var result = await _clientService.GetClientsByLastUpdate(lastUpdate);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<UpdateClientDto>>());
            Assert.That(result.Any(), Is.False);
        }
    }
}
