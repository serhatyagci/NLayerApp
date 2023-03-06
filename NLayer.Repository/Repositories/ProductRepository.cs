using Microsoft.EntityFrameworkCore;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    //cutom olduğu için hem genericrepositorydeki metodları hemde custom oluşturduğumuz iproductrepositorydeki metodları kullanıyoruz. 
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsWithCategory()
        {
            //Eager loading yani product gelirken categoryde gelir aynı anda.
            return await _context.Products.Include(x=>x.Category).ToListAsync();
        }
    }
}
