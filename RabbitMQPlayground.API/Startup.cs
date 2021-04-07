using AutoMapper;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using RabbitMQPlayground.DataLayer.Books;
using RabbitMQPlayground.DataLayer.Users;
using RabbitMQPlayground.Helpers;
using RabbitMQPlayground.Helpers.Constants;
using RabbitMQPlayground.Logic.Books.Services;
using RabbitMQPlayground.Logic.RabbitMQ;
using RabbitMQPlayground.Logic.Services.Books.Consumers;
using RabbitMQPlayground.Logic.Services.Users.Consumers;
using RabbitMQPlayground.Logic.Users.Services;
using System;
using System.Net.Http;

namespace RabbitMQPlayground.API
{
    public class Startup
    {
        private readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true })
        {
            Timeout = new TimeSpan(0, 2, 0)
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(GetType().Assembly);
            // Add MassTransit to the service collection
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<BookConsumer>();
                cfg.AddConsumer<UserConsumer>();

                cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host(new Uri(AppConfigurationBuilder.Instance.RabbitMQSettings.Host), h =>
                    {
                        h.Username(AppConfigurationBuilder.Instance.RabbitMQSettings.User);
                        h.Password(AppConfigurationBuilder.Instance.RabbitMQSettings.Password);
                    });

                    cfg.ReceiveEndpoint(RabbitMQConstants.BooksQueue, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(x => x.Interval(2, 100));
                        e.ConfigureConsumer<BookConsumer>(provider);
                        e.ExchangeType = ExchangeType.Direct;
                    });

                    cfg.ReceiveEndpoint(RabbitMQConstants.UsersQueue, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(x => x.Interval(2, 100));
                        e.ConfigureConsumer<UserConsumer>(provider);
                    });
                    //var queu = new UriBuilder(
                    //    $"rabbitmq://{Configuration["rabbitmq-credentials:host"]}/DFS_ACCOUNT_DEV1/{Configuration["rabbitmq-credentials:queue"]}");
                    //EndpointConvention.Map<IBook>(queu.Uri);
                }));
            });

            // Register IBus so that controllers can specify the dependency in the constructor
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            // Register IPublishEndpoint
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());

            // Register hosted service using the interface type IHostedService to start/stop the bus with the application
            services.AddSingleton<IHostedService, BusService>();

            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IUserService, UserService>();

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