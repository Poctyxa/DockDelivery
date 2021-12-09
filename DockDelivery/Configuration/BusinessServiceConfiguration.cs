using DockDelivery.Business.Abstract;
using DockDelivery.Business.Service;
using DockDelivery.Business.Service.Mongo;
using DockDelivery.Domain.UoW;
using Microsoft.Extensions.DependencyInjection;

namespace DockDelivery.Configuration
{
    public static class BusinessServiceConfiguration
    {
        public static void AddBusinessServiceConfig(IServiceCollection services)
        {
            //services.AddTransient<IDepartmentService, DepartmentService>(service => {
            //    return new DepartmentService(service.GetService<IDockDeliveryUnitOfWork>());
            //});

            services.AddTransient<IDepartmentService, MongoDepartmentService>();
            services.AddTransient<ICargoService, MongoCargoService>();
            services.AddTransient<ICargoTypeService, MongoCargoTypeService>();
            services.AddTransient<ICargoSectionService, MongoCargoSectionService>();
        }
    }
}
