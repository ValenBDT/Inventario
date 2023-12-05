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
        public DateTime UpdatedAt { get; set; }



    }
}