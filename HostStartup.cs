using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace WpfTest
{
    public class HostStartup
    {
        public IConfiguration Configuration { get; }

        public HostStartup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(so =>
            {
                so.IdleTimeout = TimeSpan.FromSeconds(160);
            });
            services.AddMemoryCache();
            services.AddMvc().AddSessionStateTempDataProvider(); 

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, System.IServiceProvider serviceProvider)
        {
            app.UseSession();
            app.UseMvcWithDefaultRoute();
        }
    }

}
