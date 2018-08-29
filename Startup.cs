using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using reportservice.Service;
using reportservice.Service.Interface;
using securityfilter.Services;
using securityfilter.Services.Interfaces;

namespace reportservice {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services){
            services.AddMvc();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>  {
                    builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                })
            );
            services.AddTransient<IManagerAlarmListService,ManagerAlarmListService>();
            services.AddTransient<IOtherAPIService,OtherAPIService>();
            services.AddTransient<IReportParameterServices,ReportParameterService>();
            services.AddTransient<IThingService,ThingService>();            
            services.AddTransient<IProductionOrderService,ProductionOrderService>();
            services.AddTransient<IRecipeService,RecipeService>();
            services.AddTransient<IReportAnalysisService,ReportAnalysisService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            app.UseCors ("CorsPolicy");
            app.UseForwardedHeaders (new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseMvc ();
        }
    }
}