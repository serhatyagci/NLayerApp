﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //tohumlama yani veritabanı oluşurken ilk eklenecek datalar.
            builder.HasData(new Product
            {
                id = 1,
                CadegoryId = 1,
                Name = "Kalem 1",
                Price = 100,
                Stock = 20,
                CreatedDate = DateTime.Now
            },
            new Product
            {
                id = 2,
                CadegoryId = 1,
                Name = "Kalem 2",
                Price = 200,
                Stock = 30,
                CreatedDate = DateTime.Now
            },
            new Product
            {
                id = 3,
                CadegoryId = 1,
                Name = "Kalem 3",
                Price = 600,
                Stock = 60,
                CreatedDate = DateTime.Now
            },
            new Product
            {
                id = 4,
                CadegoryId = 2,
                Name = "Kitaplar 1",
                Price = 600,
                Stock = 60,
                CreatedDate = DateTime.Now
            },
            new Product
            {
                id = 3 - 5,
                CadegoryId = 1,
                Name = "Kitaplar 2",
                Price = 600,
                Stock = 60,
                CreatedDate = DateTime.Now
            });
        }
    }
}
