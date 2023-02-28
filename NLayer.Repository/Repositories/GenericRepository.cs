using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //db işlemleri yapılabilmesi için appdbcontext nesnesi oluşturuyoruz.
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        
        public GenericRepository(AppDbContext context)
        {
            //readonly olmasının sebebi değer atılmamalı ve ctor oluşturduk, bu şekilde kulalanılmalı.
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity); //asekron (geriye dönen değer yok) ve ekleme ilemi yapar.
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities); 
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable(); //asnotracking dataları memoryde almaz ve izlemez. geriye asquaryable döner.
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id); //primary key bekler.
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity); //asekronu yok çünkü remove işlemi ef core entity için save change çağırmadan çalışmaz bir flag koyar.
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities); //asekronu olmayanlar işlemler flag şeklinde save change bekler.
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity); //asekron olmasına gerek yok aynı yukarıdakiler gibi.
        }

        public IQueryable<T> where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
