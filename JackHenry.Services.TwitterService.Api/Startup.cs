using FluentValidation;
using JackHenry.Services.TwitterService.Api.Filters;
using JackHenry.Services.TwitterService.Application.Infrastructure;
using JackHenry.Services.TwitterService.Application.Interfaces;
using JackHenry.Services.TwitterService.Application.Queries.GetTopHashtags;
using JackHenry.Services.TwitterService.Application.Queries.GetTweetCount;
using JackHenry.Services.TwitterService.Infrastructure;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Api
{
    public class Startup
    {
        private const string API_NAME = "Jack Henry Twitter Hashtags API";
        private const string API_DESCRIPTION = "RESTful API for Twitter Hashtag processing";
        private const string API_VERSION = "v1";
        private const string CORS_POLICY = "DevPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Configuring services...");

            /***************************************************
            * Services & Dependency Injection 
            ****************************************************/
            // Redis
            Log.Information("Adding Redis service.");
            var redisHost = (Environment.GetEnvironmentVariable("REDIS_HOST_NAME"));
            var redisPort = (Environment.GetEnvironmentVariable("REDIS_PORT"));
            var redisPassword = (Environment.GetEnvironmentVariable("REDIS_PASSWORD"));
            var redisConnectionUrl = $"{redisHost}:{redisPort},password={redisPassword}";
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionUrl));

            // MediatR Services
            Log.Information("Adding MediatR services.");
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(typeof(GetTweetCountRequestHandler).GetTypeInfo().Assembly);

            // Internal Services
            Log.Information("Adding Internal services.");
            services.AddSingleton(typeof(ITwitterCacheDataService), typeof(TwitterCacheDataService));
            services.AddSingleton(typeof(ITwitterStreamService), typeof(TwitterStreamService));

            // MVC Services
            Log.Information("Adding MVC services.");
            services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddControllers();

            // Fluent Validation Service
            //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetTopHashtagsRequestValidator>());
            // TODO: Replace RegisterValidatorsFromAssemblyContaining to RegisterValidatorsFromAssembly above
            // var assemblies = (System.Collections.IList)AppDomain.CurrentDomain.GetAssemblies();
            //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly();
            services.AddValidatorsFromAssemblyContaining<GetTopHashtagsRequestValidator>();

            Log.Information("Adding Swagger services.");
            // Swagger Service    
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = API_NAME,
                    Description = API_DESCRIPTION,
                    Version = API_VERSION
                });
            });

            Log.Information("Configuring services completed.");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Run the mock twitter stream service.
            var twitterStreamService = app.ApplicationServices.GetService<ITwitterStreamService>();
            twitterStreamService.Run();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", API_NAME));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
