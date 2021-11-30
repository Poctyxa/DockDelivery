using DockDelivery.Business.Abstract;
using DockDelivery.Business.Service;
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

            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<ICargoService, CargoService>();
            services.AddTransient<ICargoTypeService, CargoTypeService>();
            services.AddTransient<ICargoSectionService, CargoSectionService>();
        }
    }
}
