using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
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

//Cache ba�lant�s�.
builder.Services.AddMemoryCache();

//dinamik not foundu ba�l�yoruz.
builder.Services.AddScoped(typeof(NotFoundFilter<>));

//automapper ba�lant�s�
builder.Services.AddAutoMapper(typeof(MapProfile));

//fluent validation ba�lant�s� sa�lan�yor ve class yolu veriliyor. ayr�ca t�m controllerda kullan�labilmesi i�in options yaz�l�yor.
builder.Services.AddControllers(Options => Options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidation>());

//fluent validation filters �al��mas� i�in apinin kendi hata d�nen yap�s�n� kapatt�k.
builder.Services.Configure<ApiBehaviorOptions>( Options => 
{ 
    Options.SuppressModelStateInvalidFilter = true; 
}) ;

//Ef core'a appsettingsteki connectionu kullan diyoruz.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        //connection i�ine option(i�ine girererk) ile dinamik �ekilde appdbcontex ba�l�yoruz.
        //�ift t�rnak i�erisinde appdbcontex yaz�labilirdi aancak ad�n�n de�i�mesine kar�� dinamik bir yap� yaz�larak ba�lant� her daim g�vence alt�nda kal�yor.
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

//autofac k�t�phane ba�lant�s�
builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
//autofac modulun� ba�l�yoruz(builder.scope olanlar buraya ta��nd�.)
builder.Host.ConfigureContainer<ContainerBuilder>(ContainerBuilder => ContainerBuilder.RegisterModule(new RepoServiceModule()));


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
