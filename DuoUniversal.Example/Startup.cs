using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DuoUniversal.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var duoClientProvider = new DuoClientProvider(Configuration);
            services.AddSingleton<IDuoClientProvider>(duoClientProvider);
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }


    public interface IDuoClientProvider
    {
        public Client GetDuoClient();
    }

    internal class DuoClientProvider : IDuoClientProvider
    {
        private string ClientId { get; }
        private string ClientSecret { get; }
        private string ApiHost { get; }
        private string RedirectUri { get; }

        public DuoClientProvider(IConfiguration config)
        {
            // TODO Handle missing values
            ClientId = config.GetValue<string>("Client ID");
            ClientSecret = config.GetValue<string>("Client Secret");
            ApiHost = config.GetValue<string>("API Host");
            RedirectUri = config.GetValue<string>("Redirect URI");
        }

        public Client GetDuoClient()
        {
            return new Client(ClientId, ClientSecret, ApiHost, RedirectUri);
        }
    }
}
