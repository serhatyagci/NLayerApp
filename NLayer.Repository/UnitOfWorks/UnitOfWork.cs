using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AddDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

    internal class AppDbContext
    {
        internal void SaveChanges()
        {
            throw new NotImplementedException();
        }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public static implicit operator AppDbContext(AddDbContext v)
        {
            throw new NotImplementedException();
        }
    }
}
