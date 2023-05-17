using Levi9.POS.Domain.Models;
namespace Levi9.POS.IntegrationTests.Fixtures
{
    public static class AuthenticationFixture
    {
        public static List<Client> CreateTestClient()
        {
            var clients = new List<Client>
            {
                new Client { Id = 1, GlobalId = Guid.NewGuid(), Name = "Test1", Email = "test1@example.com", Phone = "0601111111", Address = "TestAddress 1", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password = "sptEB0EwPI/a6CnydBV/5L7TaAB47FeGjugo22djlIY=", Salt = "LGixElj/jW2XpA==" },
                new Client { Id = 2, GlobalId = Guid.NewGuid(), Name = "Test2", Email = "test2@example.com", Phone = "0602222222", Address = "TestAddress 2", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password = "NX46ovUPLLmVpNOFIFr0QDfCyVzcmYPNpdxFRzas+oI=", Salt = "k9hFapUbZ+Sd6A==" },
                new Client { Id = 3, GlobalId = Guid.NewGuid(), Name = "Test3", Email = "test3@example.com", Phone = "0603333333", Address = "TestAddress 3", LastUpdate = DateTime.Now.ToFileTimeUtc().ToString(), Password = "rBpPSZnlXxFF/duAlgKqIHFbrh/vdSerdDOUlGt/nuo=", Salt = "z0AAWyo7dDECFQ==" }
            };
            return clients;
        }
    }
}

