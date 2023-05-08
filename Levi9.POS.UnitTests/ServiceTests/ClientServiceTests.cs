using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.Domain.Models;
using Levi9.POS.Domain.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.UnitTests.ServiceTests
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _clientRepositoryMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddClientDto, Client>();
                cfg.CreateMap<Client, AddClientDto>();
            });
            _mapper = new Mapper(configurationProvider);
        }

        [Test]
        public async Task AddClient_ValidData_ReturnsAddedClient()
        {
            // Arrange
            var service = new ClientService(_clientRepositoryMock.Object, _mapper);
            var addClientDto = new AddClientDto
            {
                Name = "John",
                Address = "address",
                Email = "john.doe@example.com",
                Phone = "+3812255885"
            };
            var client = new Client
            {
                Id = 1,
                Name = "John",
                Address = "address",
                Email = "john.doe@example.com",
                Phone = "+3812255885"
            };

            _clientRepositoryMock.Setup(repo => repo.AddClient(It.IsAny<Client>())).ReturnsAsync(client);

            // Act
            var result = await service.AddClient(addClientDto);

            // Assert
            Assert.That(result.Name, Is.EqualTo(addClientDto.Name));
            Assert.That(result.Address, Is.EqualTo(addClientDto.Address));
            Assert.That(result.Email, Is.EqualTo(addClientDto.Email));
            Assert.That(result.Phone, Is.EqualTo(addClientDto.Phone));

        }
    }
}
