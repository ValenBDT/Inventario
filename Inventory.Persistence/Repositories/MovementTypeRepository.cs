using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;

namespace Inventory.Persistence.Repositories
{
    public class MovementTypeRepository: BaseRepository<MovementType>, IMovementTypeRepository
    {
        public MovementTypeRepository(DataContext context)
        :base(context)
        {
            
        }
    }
}