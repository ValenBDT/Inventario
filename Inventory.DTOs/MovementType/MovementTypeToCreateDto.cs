using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.DTOs.MovementType
{
    public class MovementTypeToCreateDTO   
    {
        public string Name { get; set; }= string.Empty;
        public string Description { get; set; }= string.Empty;
        public bool IsIncoming { get; set; }
        public bool IsOutgoing { get; set; }
        public bool IsInternalTransfer { get; set; }
    }
}