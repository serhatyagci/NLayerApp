using Autofac;
using Autofac.Core;
using NLayer.Caching;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;
namespace NLayer.API.Modules
{
    public class RepoServiceModule:Module //autofac module miras alınır.
    {
        protected override void Load(ContainerBuilder builder)
        {
            //generik eklenenler
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(Iservice<>)).InstancePerLifetimeScope();

            //tek eklenen
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            //çalışılan assemblyler alınıyor.
            var apiAssembly = Assembly.GetExecutingAssembly(); //RepoServicemodulün çalıştığı assembly alınıyor.
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext)); //verilen tipin bulunduğu repository assemblysi alınıyor.
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile)); //verilen tipin bulunduğu servise assemblysi alınıyor.


            //verilen assemblylerdeki sonu repository ve servis olanları al, as implementlerini al ve scope et.
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //InstancePerLifetimeScope => asp dat net core daki Scope karşılık geliyor.(aynı işi yapıyorlar.)
            //InstancePerDependency => transient e karşılık geliyor. (aynı işi yapıyorlar.)

            //iproduct servisi gördüğünde artık produyctservicewithcashi alıcak bağlantı.
            builder.RegisterType<ProductServiceWithCaching>().As<IProductService>(); 
        }
    }
}
