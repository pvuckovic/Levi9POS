using Levi9.POS.Domain.DTOs;

namespace Levi9.POS.Domain.Common
{
    public interface IClientService
    {
        Task<AddClientDto> AddClient(AddClientDto client);
    }
}
