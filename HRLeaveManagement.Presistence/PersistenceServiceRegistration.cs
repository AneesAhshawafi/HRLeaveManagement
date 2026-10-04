using HRLeaveManagement.Presistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HRLeaveManagement.Presistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            //regiseter the database context service with the dependency injection container 
            services.AddDbContext<HRDatabaseContext>(options =>
            {
                //with the connection string from the appsettings.json file
                options.UseSqlServer(configuration.GetConnectionString("HRDatabaseConnectionString"));
            });
            return services;
        }

    }
}
