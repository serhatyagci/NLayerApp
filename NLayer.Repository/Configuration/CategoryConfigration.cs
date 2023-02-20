using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Configuration
{
    public class CategoryConfigration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //tohumlama yani veritabanı oluşurken ilk eklenecek datalar.

            builder.HasKey(x => x.id); //birincil anahtarın id olmasını sağlar.
            builder.Property(x=>x.id).UseIdentityColumn(); //id sütunundakilerin birer birer artması içindir.
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(50); //Name alanı nullable olamaz ve max 50 karakter olabilir.

            builder.ToTable("Category"); //tablo ismi default olarak classsın ismidir ancak bu şekilde istenilen isim verilebilir.
        }
    }
}
