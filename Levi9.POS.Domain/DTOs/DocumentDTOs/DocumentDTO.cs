namespace Levi9.POS.Domain.DTOs.DocumentDTOs
{
    public class DocumentDTO
    {
        public int Id { get; set; }
        public Guid GlobalId { get; set; }
        public int ClientId { get; set; }
        public string LastUpdate { get; set; }
        public string DocumetType { get; set; }
        public string CreationDay { get; set; }
        public List<DocumentItemDTO> DocumentItems { get; set; }
    }
}
