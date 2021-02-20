using BlazorPeliculas.Client.Helpers;
using BlazorPeliculas.Client.Repositorios;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BlazorPeliculas.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //builder.Services.AddHttpClient("BlazorAutenticacion2.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            //   .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            //// Supply HttpClient instances that include access tokens when making requests to the server project
            //builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorAutenticacion2.ServerAPI"));
  
            builder.Services.AddHttpClient<HttpClientConToken>(
               cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
               .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<HttpClientSinToken>(
               cliente => cliente.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            ConfigureServices(builder.Services);

            // load language from localstorage
            builder.Services.AddLocalization();

            var host = builder.Build();
            var js = host.Services.GetRequiredService<IJSRuntime>();
            var cultura = await js.InvokeAsync<string>("cultura.get");

            if (cultura == null)
            {
                var culturaPorDefecto = new CultureInfo("es-ES");
                CultureInfo.DefaultThreadCurrentCulture = culturaPorDefecto;
                CultureInfo.DefaultThreadCurrentUICulture = culturaPorDefecto;
            }
            else
            {
                var culturaUsuario = new CultureInfo(cultura);
                CultureInfo.DefaultThreadCurrentCulture = culturaUsuario;
                CultureInfo.DefaultThreadCurrentUICulture = culturaUsuario;
            }

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddScoped<IRepositorio, Repositorio>();
            services.AddScoped<IMostrarMensajes, MostrarMensajes>();
            services.AddApiAuthorization();
            services.AddSingleton<AppState>();

        }
    }
}
