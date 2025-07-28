using Demo.BLL.Interfaces;
using Demo.BLL.Reopsitories;
using Demo.BLL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.PL.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddAplicationServices (this IServiceCollection services) 
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // FinAKHRA repositories
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IFeeRepository, FeeRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IMedicaidRequestRepository, MedicaidRequestRepository>();
            services.AddScoped<IAccountingRepository, AccountingRepository>();

            return services;
        }
    }
}
