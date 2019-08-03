using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Audit.Application.EventHandlers;
using Audit.Domain.Models;
using Audit.Infrastructure;
using Audit.Infrastructure.Repositories;
using Merlion.Core.DistributedTracing;
using Merlion.Core.Microservices.EventBus;
using Merlion.Core.Microservices.EventBus.Kafka;
using Merlion.Core.Microservices.EventBus.Kafka.AppSettings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Audit.Application
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
                        Title = "Audit API",
                        Version = "v1",
                        Description = "Through this API you can access audit functions",
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
            var connection = Configuration.GetConnectionString("AuditDBConnection");
            services.AddDbContext<RunningTotalDbContext>(optionsBuilder =>
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
            services.AddScoped<IRunningTotalRepository, RunningTotalRepository>();
            #endregion

            services.Configure<DistributedTracingOption>(Configuration.GetSection(nameof(DistributedTracingOption)));
            services.Configure<KafkaConfiguration>(Configuration.GetSection(nameof(KafkaConfiguration)));
            services.AddTransient<ISubscriber, KafkaConsumer>();
            services.AddHostedService<AuditEventHandler>();
            services.AddJaegerTracing();


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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
