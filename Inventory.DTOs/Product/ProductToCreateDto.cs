using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.DTOs.Product
{
    public class ProductToCreateDTO
    {
        public string Name { get; set; }= string.Empty;
        public string Description { get; set; }= string.Empty;
        public string BarCode { get; set; }= string.Empty;
        public decimal Price { get; set; }

        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
    }
}