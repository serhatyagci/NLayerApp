using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//unitofwork belirtiliyor. iunitofwork ile kar��la��rsa unitofwork s�n�f�n� nesne �rne�i alacak.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//generic i�in typeof ve <> olarak ekleniyor.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//servis ba�lant�s�.
builder.Services.AddScoped(typeof(Iservice<>), typeof(Service<>));

//automapper ba�lant�s�
builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
