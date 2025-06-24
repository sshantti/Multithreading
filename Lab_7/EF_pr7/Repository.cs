using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EF_pr7
{
    // Обобщенный репозиторий
    public class Repository<T> where T : class
    {
        private readonly AirplaneContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AirplaneContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Асинхронное добавление сущности
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        // Асинхронное получение всех сущностей
        public async Task<T?> GetAsync(int id) => await _dbSet.FindAsync(id);

        // Асинхронное получение всех сущностей
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        // Обновление сущности
        public void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;

        // Удаление сущности
        public void Delete(T entity) => _dbSet.Remove(entity);

        // Асинхронное сохранение изменений
        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}