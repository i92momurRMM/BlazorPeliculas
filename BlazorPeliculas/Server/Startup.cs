using AutoMapper;
using BlazorPeliculas.Server.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace BlazorPeliculas.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //Realiza mapeo de claims de client con Identity server 4.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<IdentityUser, ApplicationDbContext>()
                .AddProfileService<IdentityProfileService>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosAzStorage>();
            services.AddScoped<NotificacionesService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                //Creamos endpoints para acceder directamente a la llave publica
                endpoints.MapGet("/api/config/notificacionesllavepublica", async context =>
                {
                    var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
                    var llavePublica = configuration.GetValue<string>("notificaciones:llave_publica");
                    await context.Response.WriteAsync(llavePublica);
                });

                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
