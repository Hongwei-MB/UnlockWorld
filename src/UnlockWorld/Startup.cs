using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using UnlockWorld.Extensions;

namespace UnlockWorld
{
    /// <summary>
    /// Application startup configuration class for centralized dependency injection and configuration management
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor that receives application configuration
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Configure application services
        /// </summary>
        /// <param name="services">Service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Register configuration
            services.AddSingleton(_configuration);

            // Configure logging
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: false));

            // Register forms
            services.AddSingleton<MainForm>();

            // Use extension methods to register application services
            services
                .AddAppSettings(_configuration)
                .AddCoreServices();
        }
    }
}