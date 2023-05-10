using Levi9.POS.Domain.DTOs;

namespace Levi9.POS.Domain.Common
{
    public interface ILoginService
    {
        Task<bool> ValidateClient(ClientLogin clientLogin);
    }
}
