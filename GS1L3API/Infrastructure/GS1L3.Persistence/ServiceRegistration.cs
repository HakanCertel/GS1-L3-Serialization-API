using GS1L3.Application.IRepositories;
using GS1L3.Application.Services;
using GS1L3.Persistence.Context;
using GS1L3.Persistence.Repositories;
using GS1L3.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GS1L3.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<GS1L3DbContext>(options =>
            options.UseSqlServer(Configuration.ConnectionString));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IGs1Service, Gs1Service>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();

        }
    }
}
