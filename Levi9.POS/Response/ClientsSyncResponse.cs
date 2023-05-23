namespace Levi9.POS.WebApi.Response
{
    public class ClientsSyncResponse
    {
        public List<ClientSyncResponse> Clients { get; set; }
        public string? LastUpdate { get; set; }
    }
}
