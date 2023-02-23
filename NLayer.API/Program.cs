using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//unitofwork belirtiliyor. iunitofwork ile karþýlaþýrsa unitofwork sýnýfýný örnek alacak.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//generic için typeof ve <> olarak ekleniyor.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//Ef core'a appsettingsteki connectionu kullan diyoruz.
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        //connection içine option(içine girererk) ile dinamik þekilde appdbcontex baðlýyoruz.
        //çift týrnak içerisinde appdbcontex yazýlabilirdi ancak adýnýn deðiþmesine karþý dinamik bir yapý yazýlarak baðlantý her daim güvence altýnda kalýyor.
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
