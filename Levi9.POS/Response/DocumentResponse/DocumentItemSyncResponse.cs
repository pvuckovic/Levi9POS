﻿using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Response.DocumentResponse
{
    public class DocumentItemSyncResponse
    {
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public int Quantity { get; set; }
    }
}