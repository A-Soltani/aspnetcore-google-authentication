using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using GoogleAuthentication.Verification.Commands;
using GoogleAuthentication.Verification.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserManagement.Acl.GoogleAuthentication;

namespace GoogleAuthentication.Verification
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
            services.AddCustomSwagger()
                .AddControllers();

            services.AddTransient<GoogleAuthenticationConfig>(p => Configuration.GetSection("Authentication:Google").Get<GoogleAuthenticationConfig>());
            services.AddTransient<IGoogleLoginCallBackCommandHandler, GoogleLoginCallBackCommandHandler>();
            services
                .AddTransient<IGoogleAuthentication,
                    GoogleAuthentication.Verification.Infrastructure.GoogleAuthentication>();
        }
        private GoogleAuthenticationConfig CreateGoogleConfig(IComponentContext arg)
        {
            return Configuration.GetSection("Authentication:Google").Get<GoogleAuthenticationConfig>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Google Verification Service API");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
