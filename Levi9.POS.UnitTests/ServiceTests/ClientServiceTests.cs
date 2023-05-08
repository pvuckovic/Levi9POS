using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Service;
using Moq;
using NUnit.Framework;

namespace Levi9.POS.UnitTests.ServiceTests
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private ClientService _clientService;

        [SetUp]
        public void Setup()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _mapperMock = new Mock<IMapper>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _clientService = new ClientService(_clientRepositoryMock.Object, _mapperMock.Object, _authenticationServiceMock.Object);
        }

        [Test]
        public async Task AddClient_ValidDto_ShouldCallRepositoryAndReturnMappedDto()
        {
            var clientModel = new AddClientDto
            {
                Name = "Zlatko",
                Address = "address",
                Email = "zlatko@example.com",
                PasswordHash = "R+AKYqbYP0P/E9P1mCH5SjXOGZPbEVk79fNnUydFmWY=",
                Salt = "RZVxPvmCKmwVTA==",
                Phone = "064322222",
                LastUpdate = "634792557112051692"
            };
            _authenticationServiceMock.Setup(x => x.GenerateRandomSalt(It.IsAny<int>())).Returns(clientModel.Salt);
            _authenticationServiceMock.Setup(x => x.HashPassword(clientModel.PasswordHash, clientModel.Salt)).Returns(clientModel.PasswordHash);
            var createdClient = new AddClientDto
            {
                GlobalId = Guid.NewGuid(),
                Email = clientModel.Email,
                PasswordHash = clientModel.PasswordHash,
                Salt = clientModel.Salt,
                LastUpdate = DateTime.Now.ToFileTimeUtc().ToString()
            };
            _clientRepositoryMock.Setup(x => x.AddClient(clientModel)).Returns(createdClient);

            var result = _clientService.AddClient(clientModel);

            // Assert
            Assert.IsNotNull(result);
        }
    }

}
