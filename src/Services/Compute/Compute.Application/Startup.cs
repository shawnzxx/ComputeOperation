using System;
using System.Reflection;
using Compute.Application.Services;
using Compute.Domain.Models;
using Compute.Infrastructure;
using Compute.Infrastructure.Repositories;
using Merlion.Core.DistributedTracing;
using Merlion.Core.HttpConnectionInfo;
using Merlion.Core.Microservices.EventBus;
using Merlion.Core.Microservices.EventBus.Kafka;
using Merlion.Core.Microservices.EventBus.Kafka.AppSettings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Compute.Application
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Compute API",
                        Version = "v1",
                        Description = "Through this API you can access compute functions",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "shawn.zhang@avanade.com",
                            Name = "Shawn Zhang",
                            Url = new Uri("https://www.linkedin.com/in/shawnzxx/")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });

                //var xmlCommentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlCommentsFileFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);

                //setupAction.IncludeXmlComments(xmlCommentsFileFullPath);
            });

            #region EntityFramework Core
            var connection = Configuration.GetConnectionString("OperationDBConnection");
            services.AddDbContext<OperationDbContext>(optionsBuilder =>
            {
                optionsBuilder
                .UseSqlServer(connection, sqlServerOptionsAction: sqlOptions => {
                    sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                })
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                .EnableSensitiveDataLogging(true);
            });
            #endregion

            #region Database repository
            services.AddScoped<IOperationRepository, OperationRepository>();
            #endregion

            //register different httpClient
            services.AddHttpClient("RazerIdClient", client =>
            {
                //control HttpClient lifetimes
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            //https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            //Registering the client services as shown in the previous code, makes the DefaultClientFactory create an HttpClient configured specifically for each service
            //Just by registering your client service class with AddHttpClient(), the HttpClient object that will be injected into your class will use the configuration and policies provided upon registration. 
            //services.AddHttpClient<IAuditService, AuditService>();

            services.AddTransient<IAuditService, AuditService>();
            services.AddTransient<IPublisher, KafkaProducer>();
            services.Configure<KafkaConfiguration>(Configuration.GetSection(nameof(KafkaConfiguration))); services.Configure<DistributedTracingOption>(Configuration.GetSection(nameof(DistributedTracingOption)));
            services.AddJaegerTracing();
            services.AddConnectionInfo();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //we add at back of UseHttpsRedirection, so that all link to swagger website using http will redirect to https site
                //Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                //Enable middleware to serve Swagger UI (HTML, CSS, JS etc.)
                //Specifying the Swagger JSON endpoint
                app.UseSwaggerUI(setupAction =>
                {
                    setupAction.SwaggerEndpoint(
                        "/swagger/v1/swagger.json",
                        "Compute API V1");
                    setupAction.RoutePrefix = "";
                });
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDistributedTracing();
            app.UseConnectionInfo();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
