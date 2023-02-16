﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configuration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.id);
            builder.Property(x=>x.id).UseIdentityColumn();
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)"); //toplam 18 karakter ve virgül sonrası 2 karakter.
            builder.ToTable("Product");

            builder.HasOne(x => x.Category).WithMany(x =>x.Products).HasForeignKey(x => x.CadegoryId); //bir product'ın bir kategorisi olabilir. bir kategorinin birden fazla product'ı olabilir. foreignkey categoryid.(bunlar classlardaki tanımlamalar ve navigation properyler sayesinde kendiliğinden oluşuyor.)
        }
    }
}
