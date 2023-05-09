using Levi9.POS.Domain.DTOs;

namespace Levi9.POS.Domain.Common
{
    public interface IClientRepository
    {
        AddClientDto AddClient(AddClientDto client);
    }
}
