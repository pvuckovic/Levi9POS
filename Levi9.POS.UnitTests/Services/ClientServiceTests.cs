using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Services;
using Moq;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.Services
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private ClientService _clientService;

        [SetUp]
        public void Setup()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _mapperMock = new Mock<IMapper>();
            _clientService = new ClientService(_clientRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task AddClient_ValidDto_ShouldCallRepositoryAndReturnMappedDto()
        {
            var clientModel = new ClientDto
            {
                Name = "Zlatko",
                Address = "address",
                Email = "zlatko@example.com",
                PasswordHash = "R+AKYqbYP0P/E9P1mCH5SjXOGZPbEVk79fNnUydFmWY=",
                Salt = "RZVxPvmCKmwVTA==",
                Phone = "064322222",
                LastUpdate = "634792557112051692"
            };
            var createdClient = new ClientDto
            {
                GlobalId = Guid.NewGuid(),
                Email = clientModel.Email,
                PasswordHash = clientModel.PasswordHash,
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
    }
}
