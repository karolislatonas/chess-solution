using Chess.Api.Client;
using Chess.WebUI.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Chess.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddTransient<BoardViewModel>();
            builder.Services.AddTransient<IndexViewModel>();
            builder.Services.AddSingleton<GameService>();
            builder.Services.AddSingleton<MovementService>();

            builder.Services.AddHttpClient();

            await builder
                .Build()
                .RunAsync();
        }
    }
}
