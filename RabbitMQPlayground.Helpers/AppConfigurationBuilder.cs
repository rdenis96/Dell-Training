using Microsoft.Extensions.Configuration;
using RabbitMQPlayground.Domain.Configurations;
using System.IO;

namespace RabbitMQPlayground.Helpers
{
    public sealed class AppConfigurationBuilder
    {
        private static volatile AppConfigurationBuilder _instance;
        private static readonly object syncRoot = new object();

        public static AppConfigurationBuilder Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new AppConfigurationBuilder();
                        }
                    }
                }

                return _instance;
            }
        }

        public IConfigurationRoot Config { get; }

        public string RepositoryUrl { get { return Config.GetSection(nameof(RepositoryUrl)).Value; } }

        public MongoSettings MongoSettings
        {
            get
            {
                var settings = new MongoSettings();
                Config.GetSection(nameof(MongoSettings)).Bind(settings);
                return settings;
            }
        }

        public RabbitMQSettings RabbitMQSettings
        {
            get
            {
                var settings = new RabbitMQSettings();
                Config.GetSection(nameof(RabbitMQSettings)).Bind(settings);
                return settings;
            }
        }

        private AppConfigurationBuilder()
        {
            var envName = "Development";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (!string.IsNullOrEmpty(envName))
            {
                builder.AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true);
            }

            Config = builder.Build();
        }
    }
}