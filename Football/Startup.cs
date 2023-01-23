using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Football.Data;
using Microsoft.AspNetCore.Diagnostics;
using React.AspNet;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Football
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
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CORSPolicy",
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader(); 
                                  });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FootballApp", Version = "v1" });
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgreSQL"))
                );
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSpaStaticFiles(configuration =>
           {
               configuration.RootPath = "ClientApp/build";
           });
            services.AddReact();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FootballApp v1"));
            }
            else
            {
                app.UseExceptionHandler(
                    options =>
                    {
                        options.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                var ex = context.Features.Get<IExceptionHandlerFeature>();
                                if (ex != null)
                                {
                                    await context.Response.WriteAsync(ex.Error.Message);
                                }
                            });
                    });
                app.UseHsts();

            }
            app.UseHttpsRedirection();
            app.UseCors("CORSPolicy");
            app.UseRouting();
            app.UseStatusCodePages();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });
            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
             {
                  spa.Options.SourcePath = "ClientApp";
                 if (env.IsDevelopment())
                 {
                      spa.UseReactDevelopmentServer(npmScript: "start");
                  }
             });
        }
    }
}
