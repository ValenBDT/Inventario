using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.DTOs.InventoryStock
{
    public class InventoryStockToCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }= string.Empty;
    }
}