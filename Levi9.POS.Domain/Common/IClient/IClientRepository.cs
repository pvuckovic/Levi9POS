﻿using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.Models;

namespace Levi9.POS.Domain.Common.IClient
{
    public interface IClientRepository
    {
        ClientDto AddClient(ClientDto client);
        Task<Client> GetClientById(int id);
        Task<Client> GetClientByGlobalId(Guid id);
        Task<Client> GetClientByEmail(string email);
        Task<bool> DoesClientExist(int clientId);
    }
}
