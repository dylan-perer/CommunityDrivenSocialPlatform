using CDSP_API.Data;
using CDSP_API.Models;
using CDSP_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CDSP_API.ServiceConfig
{
    public class DbService : IServiceConfig
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("CDSPdB")).EnableSensitiveDataLogging(true));
/*
            services.AddIdentityCore<User>(options => { });*/

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ISubThreadsService, SubThreadsService>();

        }
    }
}
