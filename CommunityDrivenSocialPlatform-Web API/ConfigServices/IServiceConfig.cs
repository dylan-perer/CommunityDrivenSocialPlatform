using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CDSP_API.ServiceConfig
{
    public interface IServiceConfig
    {
        public void Configure(IServiceCollection services, IConfiguration configuration);
    }
}
