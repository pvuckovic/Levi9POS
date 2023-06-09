﻿using AutoMapper;
using Levi9.POS.Domain.Common.IClient;
using Levi9.POS.Domain.DTOs.ClientDTOs;
using Levi9.POS.Domain.Helpers;
using Levi9.POS.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Levi9.POS.Domain.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientService> _logger;
        private readonly IMapper _mapper;
        public ClientService(IClientRepository clientRepository, ILogger<ClientService> logger, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ClientDto> AddClient(ClientDto addClientDto)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientService. Timestamp: {Timestamp}.", nameof(AddClient), DateTime.UtcNow);

            addClientDto.GlobalId = Guid.NewGuid();
            string salt = AuthenticationHelper.GenerateRandomSalt();
            addClientDto.Password = AuthenticationHelper.HashPassword(addClientDto.Password, salt);
            addClientDto.Salt = salt;
            addClientDto.LastUpdate = DateTime.Now.ToFileTimeUtc().ToString();

            _clientRepository.AddClient(addClientDto);
            _logger.LogInformation("Retrieving new client in {FunctionName} of ClientService. Timestamp: {Timestamp}.", nameof(AddClient), DateTime.UtcNow);
            return addClientDto;
        }
        public async Task<ClientDto> GetClientById(int id)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientService. Timestamp: {Timestamp}.", nameof(GetClientById), DateTime.UtcNow);

            var client = await _clientRepository.GetClientById(id);
            var clientDto = _mapper.Map<ClientDto>(client);
            _logger.LogInformation("Retrieving client with ID: {Id} in {FunctionName} of ClientService. Timestamp: {Timestamp}.", id, nameof(GetClientById), DateTime.UtcNow);
            return clientDto;
        }
        public async Task<ClientDto> GetClientByGlobalId(Guid id)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientService. Timestamp: {Timestamp}.", nameof(GetClientByGlobalId), DateTime.UtcNow);
            var client = await _clientRepository.GetClientByGlobalId(id);
            var clientDto = _mapper.Map<ClientDto>(client);
            _logger.LogInformation("Retrieving client with ID: {GlobalId} in {FunctionName} of ClientService. Timestamp: {Timestamp}.", id, nameof(GetClientByGlobalId), DateTime.UtcNow);
            return clientDto;
        }
        public async Task<ClientDto> GetClientByEmail(string email)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientService. Timestamp: {Timestamp}.", nameof(GetClientByEmail), DateTime.UtcNow);
            var client = await _clientRepository.GetClientByEmail(email);
            var clientDto = _mapper.Map<ClientDto>(client);
            _logger.LogInformation("Retrieving client with Email: {Email} in {FunctionName} of ClientService. Timestamp: {Timestamp}.", email, nameof(GetClientByEmail), DateTime.UtcNow);
            return clientDto;
        }
        public bool CheckEmailExist(string email)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientService. Timestamp: {Timestamp}.", nameof(CheckEmailExist), DateTime.UtcNow);
            var result = _clientRepository.CheckEmailExist(email);
            _logger.LogInformation("Retrieving confirmation of email: {Email} existence in {FunctionName} of ClientService. Timestamp: {Timestamp}.", email, nameof(CheckEmailExist), DateTime.UtcNow);
            return result;
        }
        public async Task<UpdateClientDto> UpdateClient(UpdateClientDto client)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientService. Timestamp: {Timestamp}.", nameof(UpdateClient), DateTime.UtcNow);

            client.LastUpdate = DateTime.Now.ToFileTimeUtc().ToString();

            var clientMap = _mapper.Map<Client>(client);

            clientMap = await _clientRepository.UpdateClient(clientMap);
            _logger.LogInformation("Retrieving updated client in {FunctionName} of ClientService. Timestamp: {Timestamp}.", nameof(UpdateClient), DateTime.UtcNow);
            return client;
        }
        public async Task<IEnumerable<UpdateClientDto>> GetClientsByLastUpdate(string lastUpdate)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientService. Timestamp: {Timestamp}.", nameof(GetClientsByLastUpdate), DateTime.UtcNow);
            var clients = await _clientRepository.GetClientsByLastUpdate(lastUpdate);
            if (!clients.Any())
                return new List<UpdateClientDto>();
            _logger.LogInformation("Retrieving clients in {FunctionName} of ClientService. Timestamp: {Timestamp}.", nameof(GetClientsByLastUpdate), DateTime.UtcNow);
            return clients.Select(c => _mapper.Map<UpdateClientDto>(c)); ;
        }

        public async Task<ClientsSyncDto> SyncClients(ClientsSyncDto clientsSyncDto)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientService. Timestamp: {Timestamp}.", nameof(SyncClients), DateTime.UtcNow);
            var mapedClients = clientsSyncDto.Clients.Select(c=> _mapper.Map<Client>(c));
            List<string> lastUpdates = new List<string>();
            string lastUpdate = null;
            foreach (var client in mapedClients)
            {
                bool clientExist = await CheckClientExistence(client.GlobalId, client.Email);
                if(clientExist)
                {
                    lastUpdate = await _clientRepository.UpdateClientAsync(client);
                    lastUpdates.Add(lastUpdate);
                }
                else
                {
                    lastUpdate = await _clientRepository.InsertClientAsync(client);
                    lastUpdates.Add(lastUpdate);
                }
            }
            List<Client> clients = await _clientRepository.GetClientsWithLastUpdateGreaterThan(clientsSyncDto.LastUpdate, lastUpdates);
            var mapped = clients.Select(c => _mapper.Map<ClientSyncDto>(c)).ToList();
            _logger.LogInformation("Retrieving clients in {FunctionName} of ClientService. Timestamp: {Timestamp}.", nameof(SyncClients), DateTime.UtcNow);
            return new ClientsSyncDto() { Clients = mapped, LastUpdate = lastUpdate };
        }
        private async Task<bool> CheckClientExistence(Guid globalId, string email)
        {
            _logger.LogInformation("Entering {FunctionName} in ClientService. Timestamp: {Timestamp}.", nameof(CheckClientExistence), DateTime.UtcNow);

            bool userExists = false;

            var userByGlobalId = await _clientRepository.GetClientByGlobalId(globalId);

            if (userByGlobalId != null)
                userExists = true;
            else
            {
                var userByEmail = await _clientRepository.GetClientByEmail(email);
                if (userByEmail != null)
                    userExists = true;
            }
            _logger.LogInformation("Retrieving if clients exists in {FunctionName} of ClientService. Timestamp: {Timestamp}.", nameof(CheckClientExistence), DateTime.UtcNow);
            return userExists;
        }
    }
}
