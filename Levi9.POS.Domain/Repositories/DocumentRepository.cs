﻿using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Levi9.POS.Domain.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DataBaseContext _data;
        public DocumentRepository(DataBaseContext data) 
        {
            _data = data;
        }
        public async Task<Document> GetDocumentById(int id)
        {
            return await _data.Documents
                .Include(d => d.ProductDocuments)
                    .ThenInclude(pd => pd.Product)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Document> CreateDocument(Document newDocument)
        {
            await _data.Documents.AddAsync(newDocument);
            await _data.SaveChangesAsync();
            return newDocument;
        }
        // This will be in Clinet repository, but for now it will be here
        public async Task<bool> DoesClientExist(int clientId)
        {
            var client = await _data.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            return client != null;
        }
    }
}
