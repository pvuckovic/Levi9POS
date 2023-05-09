using Levi9.POS.Domain.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Levi9.POS.WebApi.Request
{
    public class ProductSearchRequest
    {
        public int Page { get; set; } = 1;
        public string Name { get; set; }
        [RegularExpression("^(name|id|globalId|availableQuantity)$", ErrorMessage = "The document type must be name,id,globalId,availableQuantity.")]
        public string OrderBy { get; set; } = "name";
        [RegularExpression("^(asc|dsc|ASC|DSC)$", ErrorMessage = "The document type must be asc, dsc, ASC, DSC.")]
        public string Direction { get; set; } = "asc";
    }
}
