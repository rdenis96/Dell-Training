using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQPlayground.CompositionRoot;
using RabbitMQPlayground.DataLayer.Common;
using RabbitMQPlayground.Helpers;
using RabbitMQPlayground.Logic.RabbitMQ;
using Serilog;
using System;

namespace RabbitMQPlayground.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            // Add MassTransit to the service collection
            services.AddMassTransit(cfg =>
            {
                cfg.UsingRabbitMq((provider, cfg) =>
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host(new Uri(AppConfigurationBuilder.Instance.RabbitMQSettings.Host), h =>
                    {
                        h.Username(AppConfigurationBuilder.Instance.RabbitMQSettings.User);
                        h.Password(AppConfigurationBuilder.Instance.RabbitMQSettings.Password);
                    });
                });
            });

            // Register IBus so that controllers can specify the dependency in the constructor
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            // Register IPublishEndpoint
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());

            // Register hosted service using the interface type IHostedService to start/stop the bus with the application
            services.AddSingleton<IHostedService, BusService>();
            services.AddSingleton<ICompositionRoot, CompositionRootBackend>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RabbitMQPlayground API", Version = "v1" });

                c.CustomSchemaIds(i => i.FullName);
            });

            services.AddMassTransitHostedService();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RabbitMQPlayground API");
            });

            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseCors(p => p.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });
        }
    }
}