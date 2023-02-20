using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Repository.Configuration;
using NLayer.Repository.UnitOfWorks;
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

            //seed işlemi bu şekilde de yapılabilir.(doğru olan best practies açısından burası kirlenmemelidir.)
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
            {
                id = 1,
                Color = "Kırmızı",
                Height= 100,
                Width= 200,
                ProductId= 1,
            },
            new ProductFeature()
            {
                id = 2,
                Color = "Mavi",
                Height = 300,
                Width = 400,
                ProductId = 2,
            });

            base.OnModelCreating(modelBuilder);
        }

        /*public static implicit operator AddDbContext(AppDbContext )
        {
            throw new NotImplementedException();
        }*/
    }
}
