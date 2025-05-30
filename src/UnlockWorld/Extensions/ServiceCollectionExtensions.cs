using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnlockWorld.Models;

namespace UnlockWorld.Extensions
{
    /// <summary>
    /// Extension methods for service collection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add application settings service
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // Read and register application settings from configuration
            var appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);
            services.AddSingleton(appSettings);
            
            return services;
        }

        /// <summary>
        /// Add core application services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            // Register application required services
            // Example: Register core application services
            // services.AddSingleton<ICoreService, CoreService>();
            // services.AddTransient<IDataService, DataService>();
            
            return services;
        }
    }
}