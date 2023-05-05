﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.Models
{
    public class Document
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid GlobalId { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        [StringLength(18,MinimumLength =18)]
        public string LastUpdate { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string DocumetType { get; set; }= string.Empty;
        [Required]
        [StringLength(18, MinimumLength = 18)]
        public string CreationDay { get;set; } = string.Empty;





        public Client Client { get; set; } = null!;
        public List<ProductDocument> ProductDocuments { get; set; } = new List<ProductDocument>();
    }
}