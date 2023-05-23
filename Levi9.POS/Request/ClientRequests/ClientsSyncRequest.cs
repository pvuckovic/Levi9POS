namespace Levi9.POS.WebApi.Request.ClientRequests
{
    public class ClientsSyncRequest
    {
        public List<ClientSyncRequest> Clients { get; set; }
        public string LastUpdate { get; set; }
    }
}
