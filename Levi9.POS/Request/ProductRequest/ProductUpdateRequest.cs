﻿using System.ComponentModel.DataAnnotations;
namespace Levi9.POS.WebApi.Request.ProductRequest
{
    public class ProductUpdateRequest
    {
        [Required]
        public Guid GlobalId { get; set; }
        [Required]
        public string Name { get; set; }
        public string ProductImageUrl { get; set; }
        public int AvailableQuantity { get; set; }
        [Required]
        public string LastUpdate { get; set; }
        public float Price { get; set; }
    }
}
