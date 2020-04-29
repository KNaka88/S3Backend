using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace S3Backend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public readonly string LocalhostOrigins = "local_development";

        public Startup(IWebHostEnvironment env)
        {
            WebHostEnvironment = env;

            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                            .AddJsonFile("version.json", optional: true)
                            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                var parseSuccess = bool.TryParse(Environment.GetEnvironmentVariable("USE_LOCAL_STACK"), out var useLocalStack);
                if (parseSuccess && useLocalStack)
                {
                    services.ConfigureLocalStack();
                }
                else
                {
                    services.ConfigureAWS();
                }

                services.AddCors(options =>
                {
                    options.AddPolicy(name: LocalhostOrigins, builder =>
                    {
                        builder.WithOrigins("https://localhost:3000")
                                .AllowAnyHeader();
                    });
                });
            }
            else
            {
                services.ConfigureAWS();
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "S3Backend", Version = "v1" });
                var filePath = AppContext.BaseDirectory + "S3Backend.xml";
                c.IncludeXmlComments(filePath);
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(LocalhostOrigins);
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
