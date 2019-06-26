using Amazon.DynamoDBv2;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace w_rsvp.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                var config = new AmazonDynamoDBConfig
                {
                    ServiceURL = "http://localhost:5155/"
                };

                services.AddScoped<IAmazonDynamoDB>(sp => new AmazonDynamoDBClient("fake", "fake", config));
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(opts => 
                {
                    opts.AllowAnyHeader();
                    opts.AllowAnyMethod();
                    opts.AllowAnyOrigin();
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
