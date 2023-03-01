﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configuration
{
    internal class ProductFutureConfiguration : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x=>x.Product).WithOne(x=>x.ProductFeature).HasForeignKey<ProductFeature>(x=>x.ProductId); //bire bir ilişki ve bire bir ilişki olduğundan foreigntkeyin hangi tabloda olacağı bilgisi verilmeli. ayrıca bu ilişkide classlarda oluşturuldu otomatik.
        }
    }
}
