namespace Inventory.DTOs.Categoty
{
    public class CategoryToEditDTO
    {
        public int Id { get; set; }   
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
    }
}