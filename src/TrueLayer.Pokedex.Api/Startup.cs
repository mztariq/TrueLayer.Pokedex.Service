using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using TrueLayer.Pokedex.Api.Middleware;
using TrueLayer.Pokedex.Application.Services;
using TrueLayer.Pokedex.Domain.Models;
using TrueLayer.Pokedex.Infrastructure.HttpClients;

namespace TrueLayer.Pokedex.Api
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
            services.AddControllers();
            services.AddHealthChecks();
            services.AddLogging();
            AddDependencies(services);
            AddSwagger(services);
            AddApiVersioning(services);
            AddPokemonHttpClient(services);
            AddTranslatorHttpClient(services);
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            const string healthCheckUrl = "/healthcheck";
            const string swaggerApiName = "TrueLayer Pokedex Service";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection(); 

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"{swaggerApiName} - {description.GroupName.ToUpper()}");
                }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks(healthCheckUrl, new HealthCheckOptions
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                });
            });
        }

        private void AddDependencies(IServiceCollection services)
        {
            services.AddScoped<IPokemonService, PokemonService>();
            services.AddScoped<ITranslationService, TranslationService>();
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options => { options.OperationFilter<SwaggerDefaultValues>(); });
        }

        private static void AddApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
        }

        private void AddPokemonHttpClient(IServiceCollection services)
        {
            var options = Configuration.GetSection("PokemonSettings").Get<PokemonSettings>();
            services.AddHttpClient<IPokemonHttpClient, PokemonHttpClient>(c =>
                {
                    c.BaseAddress = new Uri(options.ApiBaseUri);
                });
        }

        private void AddTranslatorHttpClient(IServiceCollection services)
        {
            var options = Configuration.GetSection("TranslationSettings").Get<TranslationSettings>();
            services.AddHttpClient<ITranslationHttpClient, TranslationHttpClient>(c =>
                {
                    c.BaseAddress = new Uri(options.ApiBaseUri);
                });
        }
    }
}
