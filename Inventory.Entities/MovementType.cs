namespace Inventory.Entities
{
    public class MovementType
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public bool IsInComing { get; set; }
        public bool IsOutgoing { get; set; }
        public bool IsInternarTransfer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public IEnumerable<InventoryMovement>? InventoryMovements { get; set; }
    }
}