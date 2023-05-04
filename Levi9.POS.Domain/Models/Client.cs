using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.Models
{
    
    public class Client
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid GlobalId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;
        [Required]       
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Phone { get; set; } = string.Empty;
        [Required]
        [StringLength(18,MinimumLength =18)]
        public string LastUpdate { get; set; } = string.Empty;


        public List<Document> Documents { get; set; } = new List<Document>();
    }
}
