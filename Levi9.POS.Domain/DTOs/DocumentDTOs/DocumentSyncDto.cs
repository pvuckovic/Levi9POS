namespace Levi9.POS.Domain.DTOs.DocumentDTOs
{
    public class DocumentSyncDto
    {
        public Guid GlobalId { get; set; }
        public Guid ClientId { get; set; }
        public string DocumentType { get; set; }
        public List<DocumentItemSyncDto> Items { get; set; }
    }
}