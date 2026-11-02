using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRLeaveManagement.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add application services here
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            // Source - https://stackoverflow.com/q/64040375
            // Posted by Emba Ayyub, modified by community. See post 'Timeline' for change history
            // Retrieved 2026-11-02, License - CC BY-SA 4.0

            return services;
        }
    }
}
