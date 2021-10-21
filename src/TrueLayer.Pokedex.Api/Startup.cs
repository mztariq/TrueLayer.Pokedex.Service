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
using Otb.AspNetCore.Utilities.Middleware;
using TrueLayer.Pokedex.Api.Middleware;
using TrueLayer.Pokedex.Application.Services;
using TrueLayer.Pokedex.Domain.Models;
using TrueLayer.Pokedex.Infrastructure.HttpClients;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Net.Http;

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
            AddSampleHttpClient(services); // We have sample service inject, which can be replaced with your choice of service required for the project or can be completely removed (if not required)
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            const string healthCheckUrl = "/healthcheck";
            const string swaggerApiName = "$safeprojectname$ API";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseNewRelicIgnoreTransaction(healthCheckUrl, "/metrics");

            //app.UseHttpsRedirection(); removing this as our kube pods dont support https routing. If you want to add it just un comment, else remove this line

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
            services.AddSingleton<ISampleService, SampleService>();
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

        private void AddSampleHttpClient(IServiceCollection services) // This can be updated to use your required Httpclient
        {
            var options = Configuration.GetSection("SampleSettings").Get<SampleSettings>();
            services.AddHttpClient<ISampleHttpClient, SampleHttpClient>(c =>
                {
                    c.BaseAddress = new Uri(new Uri(options.ApiBaseUri), options.ApiRelativeUri);
                })
                .AddPolicyHandler(GetRetryPolicy(options))
                .AddPolicyHandler(GetCircuitBreakerPolicy(options));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(SampleSettings options) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => (int)response.StatusCode == 429)
                .WaitAndRetryAsync(options.DefaultMaxRetryAttempts,
                    retryAttempt => TimeSpan.FromSeconds(options.DefaultWaitTimeInSecondsBetweenAttempts));

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(SampleSettings options) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => (int)response.StatusCode == 429)
                .CircuitBreakerAsync(options.DefaultMaxRetryAttempts,
                    TimeSpan.FromSeconds(options.DefaultCircuitBreakerTimeInSeconds));
    }
}
