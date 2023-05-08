using AutoMapper;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.DTOs;
using Levi9.POS.WebApi.Controllers;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.UnitTests.ControllerTests
{
    public class ClientControllerTest
    {
        [Test]
        public async Task AddClient_ReturnsCreatedAtAction()
        {
            // Arrange
            var addClientDto = new AddClientDto 
            {
                Name = "Test",
                Address = "Address",
                Phone = "+385111111",
                Email = "aaa@gmail.com"
            };
            var expectedAddClientResponse = new AddClientResponse 
            {
                Id = 1,
                GlobalId = Guid.NewGuid(),
                Name = "Test",
                Address = "Address",
                Phone = "+385111111",
                Email = "aaa@gmail.com",
                LastUpdate = DateTime.Now.ToFileTimeUtc().ToString()
            };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<AddClientResponse>(It.IsAny<AddClientDto>()))
                      .Returns(expectedAddClientResponse);

            var mockClientService = new Mock<IClientService>();
            mockClientService.Setup(m => m.AddClient(It.IsAny<AddClientDto>()));

            var controller = new ClientController(mockClientService.Object, mockMapper.Object);

            // Act
            var result = await controller.AddClient(addClientDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);

            var createdAtActionResult = (CreatedAtActionResult)result.Result;
            Assert.That(createdAtActionResult.ActionName, Is.EqualTo("addClientResponse"));

            var value = (AddClientResponse)createdAtActionResult.Value;
            Assert.That(value, Is.EqualTo(expectedAddClientResponse));
        }
    }
}
