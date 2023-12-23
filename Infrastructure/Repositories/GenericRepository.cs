using Core.Context;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task Add(T entity)
            => await _context.Set<T>().AddAsync(entity);

        public void Delete(T entity)
            => _context.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int? id)
            => await _context.Set<T>().FindAsync(id);

        public void Update(T entity)
            => _context.Set<T>().Update(entity);

        public async Task<T> GetEntityWithSpecificationsAsync(ISpecifications<T> specifications)
            => await ApplySpecifications(specifications).FirstOrDefaultAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationsAsync(ISpecifications<T> specifications)
            => await ApplySpecifications(specifications).ToListAsync();

        public async Task<int> CountAsync(ISpecifications<T> specifications)
            => await ApplySpecifications(specifications).CountAsync();

        private IQueryable<T> ApplySpecifications(ISpecifications<T> specifications)
            => SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specifications);
    }
}
