using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChatKid.DataLayer
{
    public static class Registrations
    {
        public static IServiceCollection RegisterDataLayer(this IServiceCollection services)
        {
            services.AddScoped<IDBContext, ApplicationDBContext>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(EfRepository<>));
            
            return services;
        }
    }
}
