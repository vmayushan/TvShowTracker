using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using TvShowTracker.DataAccess.OMDb;
using TvShowTracker.DataAccess.Storage.CollectionsSetup;
using TvShowTracker.DataAccess.Storage.Dao;
using TvShowTracker.Domain.Services;
using TvShowTracker.WebApi.Configuration;
using TvShowTracker.WebApi.HostedServices;
using TvShowTracker.WebApi.HttpClientHandlers;
using TvShowTracker.WebApi.Services;

namespace TvShowTracker.WebApi
{
    public class Startup
    {
        private const string CorsLocalPolicy = "CorsLocalPolicy";
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            var omdbApiSettings = Configuration
                .GetSection(OMDbApiSettings.ConfigSectionKey)
                .Get<OMDbApiSettings>();
            
            services.Configure<MongoSettings>(Configuration.GetSection(MongoSettings.ConfigSectionKey));
            services.Configure<OMDbApiSettings>(Configuration.GetSection(OMDbApiSettings.ConfigSectionKey));

            services.AddScoped<IShowDao, ShowDao>();
            services.AddScoped<IShowProgressDao, ShowProgressDao>();
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserShowService, UserShowService>();
            services.AddScoped<IShowProgressService, ShowProgressService>();
            services.AddScoped<IOMDbShowImportService, OMDbShowImportService>();
            
            services.AddSingleton<IMongoCollectionSetup, ShowCollectionSetup>();
            services.AddSingleton<IMongoCollectionSetup, ShowProgressCollectionSetup>();
            services.AddHostedService<MongoSetupHostedService>();
            
            services.AddScoped<OMDbApiKeyHandler>();
            services
                .AddHttpClient<IOMDbShowClient, OMDbShowClient>(client =>
                {
                    client.BaseAddress = new Uri(omdbApiSettings.BaseAddress);
                })
                .AddHttpMessageHandler<OMDbApiKeyHandler>();
            
            services.AddCors(options => options.AddPolicy(CorsLocalPolicy, builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(
                        "http://localhost:3001", 
                        "https://localhost:3001"
                    );
            }));
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            var domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Show Tracker API", Version = "v1" });
            });

            ConfigureStorageServices(services);
            
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(CorsLocalPolicy);
                
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Show Tracker API");
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();
            

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private void ConfigureStorageServices(IServiceCollection services)
        {
            // ReSharper disable once RedundantTypeArgumentsOfMethod
            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var mongoSettings = sp.GetRequiredService<IOptions<MongoSettings>>();
                
                var conventionPack = new ConventionPack { new CamelCaseElementNameConvention(), new StringObjectIdIdGeneratorConvention() };
                ConventionRegistry.Register(nameof(conventionPack), conventionPack, t => true);

                var mongoConnectionString = mongoSettings.Value.ConnectionString
                                            ?? throw new InvalidOperationException("Empty MongoDb connection string");
                
                var databaseName = mongoSettings.Value.DatabaseName 
                                   ?? throw new InvalidOperationException("Empty MongoDb database name");
                
                var client = new MongoClient(mongoConnectionString);
                return client.GetDatabase(databaseName);
            });
        }
    }
}