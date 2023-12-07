using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;

namespace Inventory.Persistence.Repositories
{
    public class SupplierRepository: BaseRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(DataContext context)
        :base(context)
        {
            
        }
    }
}