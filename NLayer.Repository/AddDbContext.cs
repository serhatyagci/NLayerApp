using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AddDbContext : DbContext
    {

        public AddDbContext(DbContextOptions<AddDbContext> options):base(options)
        {

        }
        //datalar set ediliyor yani bağlanıyor.
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductFeature> productsFeature { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configurationlar burada topanır.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //tüm configuration kullanan classları okur ve çalışılan assembly taranır.
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());  //tek tek bu şekilde de tanımlanabilir.
            base.OnModelCreating(modelBuilder);
        }
    }
}
