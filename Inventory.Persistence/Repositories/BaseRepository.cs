using Inventory.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>, IDisposable
    where T: class
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> AddAllAsync(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _dbSet.FindAsync(id);

            if(item is not null){
                _context.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var items = await _dbSet.ToListAsync();
            return items;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var item = await _dbSet.FindAsync(id);
            return item;

        }

        public async Task<bool> UpdateAsync(int id, T entity)
        {
            var item = await _dbSet.FindAsync(id);

            if(item is not null){
                _context.Update(item);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public void Dispose(){
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}