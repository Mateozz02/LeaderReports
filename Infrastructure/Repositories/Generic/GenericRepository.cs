using LeaderReport.Data;
using Microsoft.EntityFrameworkCore;

namespace LeaderReport.Infrastructure.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly LeaderReportContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(LeaderReportContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll() => await _dbSet.ToListAsync();
        public async Task<T> GetById(int id) => await _dbSet.FindAsync(id);
        public async Task Add(T entidad) { await _dbSet.AddAsync(entidad); await _context.SaveChangesAsync(); }
        public async Task Update(T entidad) { _dbSet.Update(entidad); await _context.SaveChangesAsync(); }
        public async Task Delete(int id)
        {
            var entidad = await _dbSet.FindAsync(id);
            if (entidad != null) _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }
    }

}
