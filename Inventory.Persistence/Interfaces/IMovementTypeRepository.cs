using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Entities;
using Inventory.Persistence.Repositories;

namespace Inventory.Persistence.Interfaces
{
    public interface IMovementTypeRepository: IBaseRepository<MovementType>
    {
        
    }
}