using Levi9.POS.Domain;
using Levi9.POS.Domain.Helpers;
using Levi9.POS.Domain.Models;
using Levi9.POS.WebApi.Request.ClientRequests;
using Levi9.POS.WebApi.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Levi9.POS.IntegrationTests.Controllers
{
    [TestFixture]
    public class ClientControllerIntegrationTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        string token = "";

        [SetUp]
        public void SetUp()
        {
            token = AuthenticationHelper.GenerateJwtTestCase();
            _factory = new CustomWebAppFactory<Program>();
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [Test]
        public async Task AddClient_ValidRequest_ShouldReturnOk()
        {
            var clientRequest = new ClientRequest
            {
                Address = "address",
                Email = "email@gmail.com",
                Name = "name",
                Password = "password",
                Phone = "1234567890"
            };

            var response = await _client.PostAsJsonAsync("v1/client/", clientRequest);
            var result = await response.Content.ReadAsStringAsync();
            var clientResponse = JsonConvert.DeserializeObject<ClientResponse>(result);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientRequest.Name, Is.EqualTo(clientResponse.Name));
            Assert.That(clientRequest.Email, Is.EqualTo(clientResponse.Email));
        }
        [Test]
        public async Task AddClient_EmailExist_ShouldReturnBadRequest()
        {
            var clientRequest = new ClientRequest
            {
                Address = "address",
                Email = "example@gmail.com",
                Name = "name",
                Password = "password",
                Phone = "1234567890"
            };

            var response = await _client.PostAsJsonAsync("v1/client/", clientRequest);
            var result = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(result, Is.EqualTo("Email already exists!"));
        }
        [Test]
        public async Task UpdateClient_ValidRequest_ShouldReturnOk()
        {
            var clienUpdate = new ClientUpdate
            {
                Id = 1,
                GlobalId = new Guid("10bc28f5-7042-4736-97ad-1cb3dce98b1c"),
                Address = "address",
                Email = "email123@gmail.com",
                Name = "name",
                Phone = "1234567890"
            };

            var response = await _client.PutAsJsonAsync("v1/client/update", clienUpdate);
            var result = await response.Content.ReadAsStringAsync();
            var clientResponse = JsonConvert.DeserializeObject<ClientResponse>(result);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clienUpdate.Name, Is.EqualTo(clientResponse.Name));
            Assert.That(clienUpdate.Email, Is.EqualTo(clientResponse.Email));
        }
        [Test]
        public async Task UpdateClient_EmailExist_ShouldReturnBadRequest()
        {
            var clienUpdate = new ClientUpdate
            {
                Id = 1,
                GlobalId = new Guid("10bc28f5-7042-4736-97ad-1cb3dce98b1c"),
                Address = "address",
                Email = "example@gmail.com",
                Name = "name",
                Phone = "1234567890"
            };

            var response = await _client.PutAsJsonAsync("v1/client/update", clienUpdate);
            var result = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(result, Is.EqualTo("Email already exists!"));
        }
        [Test]
        public async Task GetClientById_ShouldReturnClientIfExist()
        {
            int id = 1;

            var response = await _client.GetAsync($"v1/client/{id}");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Client>(content);
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(id));
        }
        [Test]
        public async Task GetClientById_InvalidId_ReturnsBadRequest()
        {
            int id = 0;

            var response = await _client.GetAsync($"/v1/client/{id}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task GetClientByNegativeId_ShouldReturnBadRequest()
        {
            int id = -1;

            var response = await _client.GetAsync($"v1/client/{id}");

            var result = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(result, Is.EqualTo("Id must be a positive number"));
        }
        [Test]
        public async Task GetClientById_NoClientFound_ReturnsNotFound()
        {
            int id = 100000;

            var response = await _client.GetAsync($"/v1/client/{id}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task GetClientById_Unauthorized_ReturnsUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", string.Empty);
            int id = 1;

            var response = await _client.GetAsync($"/v1/client/{id}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task GetClientByGlobalId_ShouldReturnClientIfExist()
        {
            var globalId = "10bc28f5-7042-4736-97ad-1cb3dce98b1c";
            string name = "Marko";

            var response = await _client.GetAsync($"v1/client/global/{globalId}");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ClientResponse>(content);
            Assert.NotNull(result);
            Assert.That(name, Is.EqualTo(result.Name));
        }
        [Test]
        public async Task GetClientByGlobalId_NoClientFound_ReturnsNotFound()
        {
            var globalId = new Guid();

            var response = await _client.GetAsync($"/v1/client/global/{globalId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task GetClientByGlobalId_Unauthorized_ReturnsUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", string.Empty);

            var response = await _client.GetAsync("/v1/client/global/10bc28f5-7042-4736-97ad-1cb3dce98b1c");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task GetAllClients_ReturnsOkWithMappedList_WhenServiceReturnsNonEmptyList()
        {
            var response = await _client.GetAsync("/v1/Client/sync/133288706851213387");

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IEnumerable<ClientResponse>>(content);
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(result, Is.Not.Null);
            });
        }
        [Test]
        public async Task GetAllClients_ReturnsOkWithEmptyList_WhenServiceReturnsEmptyList()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
                dbContext.Clients.RemoveRange(dbContext.Clients);
                dbContext.SaveChanges();
            }
            var response = await _client.GetAsync("/v1/Client/sync/933288706851213387");

            var content = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(content, Is.EqualTo("[]"));
            });
        }
        [Test]
        public async Task SyncClients_ValidRequest_ReturnsOkWithSyncedResponse()
        {
            var clientRequest = new ClientsSyncRequest
            {
                Clients = new List<ClientSyncRequest>
                {
                    new ClientSyncRequest
                    {
                        GlobalId = Guid.NewGuid(),
                        Name = "Petar",
                        Address = "Novosadskog sajma 11",
                        Email = "petar@example.com",
                        Phone = "1234567890",
                        Password = "password123",
                        LastUpdate = "123456789987654321",
                        Salt = "somesalt"
                    }
                },
                LastUpdate = "123456789987654321"
            };

            var expectedResponse = new ClientsSyncResponse
            {
                 Clients = new List<ClientSyncResponse>
                 {
                     new ClientSyncResponse
                     {
                        GlobalId = new Guid("10bc28f5-7042-4736-97ad-1cb3dce98b1c"),
                         Name = "Marko",
                         Email = "example@gmail.com",
                         Phone = "+387 65 132 527",
                         Address = "1.maja, Derventa",
                         LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(),
                         Password ="password", Salt="Salt"
                     },
                     new ClientSyncResponse
                     {
                         GlobalId = new Guid("aa7ac410-5b2a-497e-8106-266c09f705ae"),
                         Name = "Aleksa", 
                         Email = "aleksa@gmail.com", 
                         Phone = "+387 64 862 476",
                         Address = "Koste Racina 24, Novi Sad", 
                         LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), 
                         Password = "password123", 
                         Salt="Salt123"
                     },
                     new ClientSyncResponse
                     {
                         GlobalId = new Guid("5f8ac59b-1604-48fe-bcd3-7d8dbf70db08"), 
                         Name = "Milos", 
                         Email = "milos@gmail.com", 
                         Phone = "+387 65 912 127", 
                         Address = "Strumicka 13, Novi Sad",
                         LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), 
                         Password = "password123", Salt = "Salt123" 
                     }
                 },
                 LastUpdate = "123456789987654321"
            };

            var serializedRequest = JsonConvert.SerializeObject(clientRequest);
            var content = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("v1/Client/sync", content);
            var result = await response.Content.ReadAsStringAsync();
            var clientResponse = JsonConvert.DeserializeObject<ClientsSyncResponse>(result);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(clientResponse.Clients, Has.Count.EqualTo(expectedResponse.Clients.Count));
            });
        }
        [Test]
        public async Task SyncClients_ValidRequest_EmptyClients_ReturnsOkWithSyncedResponseLastUpdateTheSame()
        {
            var clientRequest = new ClientsSyncRequest
            {
                Clients = new List<ClientSyncRequest>(),
                LastUpdate = "123456789987654321"
            };

            var expectedResponse = new ClientsSyncResponse
            {
                Clients = new List<ClientSyncResponse> {
                     new ClientSyncResponse
                     {
                         GlobalId = new Guid("10bc28f5-7042-4736-97ad-1cb3dce98b1c"),
                         Name = "Marko",
                         Email = "example@gmail.com",
                         Phone = "+387 65 132 527",
                         Address = "1.maja, Derventa",
                         LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(),
                         Password ="password", Salt="Salt"
                     },
                     new ClientSyncResponse
                     {
                         GlobalId = new Guid("aa7ac410-5b2a-497e-8106-266c09f705ae"),
                         Name = "Aleksa",
                         Email = "aleksa@gmail.com",
                         Phone = "+387 64 862 476",
                         Address = "Koste Racina 24, Novi Sad",
                         LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(),
                         Password = "password123",
                         Salt="Salt123"
                     },
                     new ClientSyncResponse
                     {
                         GlobalId = new Guid("5f8ac59b-1604-48fe-bcd3-7d8dbf70db08"),
                         Name = "Milos",
                         Email = "milos@gmail.com",
                         Phone = "+387 65 912 127",
                         Address = "Strumicka 13, Novi Sad",
                         LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(),
                         Password = "password123", Salt = "Salt123"
                     }
                 },
                LastUpdate = "123456789987654321"
            };

            var serializedRequest = JsonConvert.SerializeObject(clientRequest);
            var content = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("v1/Client/sync", content);
            var result = await response.Content.ReadAsStringAsync();
            var clientResponse = JsonConvert.DeserializeObject<ClientsSyncResponse>(result);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(clientResponse.Clients, Has.Count.EqualTo(expectedResponse.Clients.Count));
                Assert.That(clientResponse.LastUpdate, Is.EqualTo(null));
            });
        }
        [Test]
        public async Task SyncClients_ValidRequest_ReturnsOkWithSyncedResponseLast_ClientsCountTwo()
        {
            var clientRequest = new ClientsSyncRequest
            {
                Clients = new List<ClientSyncRequest>
                {
                    new ClientSyncRequest
                    {
                        GlobalId = new Guid("10bc28f5-7042-4736-97ad-1cb3dce98b1c"),
                        Name = "Petar",
                        Address = "Novosadskog sajma 11",
                        Email = "petar@example.com",
                        Phone = "1234567890",
                        Password = "password123",
                        LastUpdate = "123456789987654321",
                        Salt = "somesalt"
                    }
                },
                LastUpdate = "123456789987654321"
            };

            var expectedResponse = new ClientsSyncResponse
            {
                Clients = new List<ClientSyncResponse> {
                     new ClientSyncResponse
                     {
                         GlobalId = new Guid("aa7ac410-5b2a-497e-8106-266c09f705ae"),
                         Name = "Aleksa",
                         Email = "aleksa@gmail.com",
                         Phone = "+387 64 862 476",
                         Address = "Koste Racina 24, Novi Sad",
                         LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(),
                         Password = "password123",
                         Salt="Salt123"
                     },
                     new ClientSyncResponse
                     {
                         GlobalId = new Guid("5f8ac59b-1604-48fe-bcd3-7d8dbf70db08"),
                         Name = "Milos",
                         Email = "milos@gmail.com",
                         Phone = "+387 65 912 127",
                         Address = "Strumicka 13, Novi Sad",
                         LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(),
                         Password = "password123", Salt = "Salt123"
                     }
                 },
                LastUpdate = "123456789987654321"
            };

            var serializedRequest = JsonConvert.SerializeObject(clientRequest);
            var content = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("v1/Client/sync", content);
            var result = await response.Content.ReadAsStringAsync();
            var clientResponse = JsonConvert.DeserializeObject<ClientsSyncResponse>(result);
            expectedResponse.LastUpdate = clientResponse.LastUpdate;
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(clientResponse.Clients, Has.Count.EqualTo(expectedResponse.Clients.Count));
            });
        }
    }
}
