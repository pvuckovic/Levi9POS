﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levi9.POS.Domain.DTOs.ProductDTOs
{
    public class ProductSyncRequestDTO
    {
        public Guid GlobalId { get; set; }
        public string Name { get; set; }
        public string ProductImageUrl { get; set; }
        public int AvailableQuantity { get; set; }
        public string LastUpdate { get; set; }
        public float Price { get; set; }
    }
}
