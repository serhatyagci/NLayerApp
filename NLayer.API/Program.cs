using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//unitofwork belirtiliyor. iunitofwork ile karþýlaþýrsa unitofwork sýnýfýný nesne örneði alacak.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//generic için typeof ve <> olarak ekleniyor.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//servis baðlantýsý.
builder.Services.AddScoped(typeof(Iservice<>), typeof(Service<>));

//automapper baðlantýsý
builder.Services.AddAutoMapper(typeof(MapProfile));

//fluent validation baðlantýsý saðlanýyor ve class yolu veriliyor. ayrýca tüm controllerda kullanýlabilmesi için options yazýlýyor.
builder.Services.AddControllers(Options => Options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidation>());

//fluent validation filters çalýþmasý için apinin kendi hata dönen yapýsýný kapattýk.
builder.Services.Configure<ApiBehaviorOptions>( Options => 
{ 
    Options.SuppressModelStateInvalidFilter = true; 
}) ;

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

//Ef core'a appsettingsteki connectionu kullan diyoruz.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        //connection içine option(içine girererk) ile dinamik þekilde appdbcontex baðlýyoruz.
        //çift týrnak içerisinde appdbcontex yazýlabilirdi aancak adýnýn deðiþmesine karþý dinamik bir yapý yazýlarak baðlantý her daim güvence altýnda kalýyor.
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomException(); //kendi custom middleware aktif hale getiriyoruz.

app.UseAuthorization();

app.MapControllers();

app.Run();
