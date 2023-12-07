using Inventory.Entities;
using Inventory.Persistence.Interfaces;

namespace Inventory.Persistence.Repositories
{
    public class CategoryRepository: BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext context)
        :base(context)
        {
            
        }
    }
}