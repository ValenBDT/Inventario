using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.DTOs.Product
{
    public class ProductToListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public string Description { get; set; }= string.Empty;
        public string BarCode { get; set; }= string.Empty;
        public decimal Price { get; set; }

        public string SupplierName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}