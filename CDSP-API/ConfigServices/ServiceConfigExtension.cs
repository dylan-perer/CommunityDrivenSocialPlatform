using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CDSP_API.ServiceConfig
{
    public static class ServiceConfigExtension
    {
        public static void ConfigureAll(IServiceCollection services, IConfiguration configuration)
        {
            var serviceConfigs = typeof(Startup).Assembly.ExportedTypes.Where(t =>
                typeof(IServiceConfig).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IServiceConfig>().ToList();

            serviceConfigs.ForEach(service => service.Configure(services, configuration));
        }
    }
}
