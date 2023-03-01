using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base (options)
        {

        }


        //db set ediliyor.
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }


        //Entity ayarlarının yapıldığı yer. ancak configurationlar ayrı klaslarda yapılıp buraya çekilir)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  //çalıştığımız assembly içindeki configuration clasllarını çeker.

            //modelBuilder.ApplyConfiguration(new ProductConfiguration());   //tek tek bu şekilde de eklenebilir.


            //seeds işlemi burada da yapılabilir.
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
            {
                Id=1,
                Color="Kırmızı",
                Height=100,
                Width=200,
                ProductId=1
            },
            new ProductFeature()
            {
                Id = 2,
                Color = "Mavi",
                Height = 300,
                Width = 500,
                ProductId = 2
            });

            base.OnModelCreating(modelBuilder); 
        }
    }
}
