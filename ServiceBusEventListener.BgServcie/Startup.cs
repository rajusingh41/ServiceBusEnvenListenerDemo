using Microsoft.AspNetCore.Builder;
using System.Diagnostics.CodeAnalysis;

namespace ServiceBusEventListener.BgServcie
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
      public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; 
        }

        public void ConfigureServices(IServiceCollection services) { }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) { }
    }
}
