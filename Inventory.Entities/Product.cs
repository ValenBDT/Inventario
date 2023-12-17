namespace Inventory.Entities
{
    public class Product
    {
        public int Id { get; set; }   
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string BarCode { get; set; } = String.Empty;
        public Decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        //public IEnumerable<InventoryStock> InventoryStocks { get; set; } es 1 a 1(?)
        public IEnumerable<InventoryMovement>? InventoryMovements { get; set; }

    }
}