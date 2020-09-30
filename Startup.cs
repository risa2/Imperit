using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Imperit
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public static void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddRazorPages();
            _ = services.AddServerSideBlazor();

            _ = services.AddSingleton<Services.IServiceIO>(s => new Services.ServiceIO(new Load.File("./Files/Settings.json"), new Load.File("./Files/Players.json"), new Load.File("./Files/Provinces.json"), new Load.File("./Files/Actions.json"), new Load.File("./Files/Events.json"), new Load.File("./Files/Active.json"), new Load.File("./Files/Password.txt"), new Load.File("./Files/Graph.json"), new Load.File("./Files/Mountains.json"), new Load.File("./Files/Shapes.json"), new Load.File("Files/Powers.json")))
                    .AddSingleton<Services.IActionLoader, Services.ActionLoader>()
                    .AddSingleton<Services.ISettingsLoader, Services.SettingsLoader>()
                    .AddSingleton<Services.IPlayersLoader, Services.PlayersLoader>()
                    .AddSingleton<Services.IProvincesLoader, Services.ProvincesLoader>()
                    .AddSingleton<Services.IPowersLoader, Services.PowersLoader>()
                    .AddTransient<Services.IActivePlayer, Services.ActivePlayer>()
                    .AddTransient<Services.INewGame, Services.NewGame>()
                    .AddTransient<Services.IEndOfTurn, Services.EndOfTurn>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _ = env.IsDevelopment() ? app.UseDeveloperExceptionPage() : app.UseExceptionHandler("/Error").UseHsts();

            _ = app.UseHttpsRedirection().UseStaticFiles().UseRouting()
                .UseEndpoints(endpoints =>
                {
                    _ = endpoints.MapBlazorHub();
                    _ = endpoints.MapFallbackToPage("/_Host");
                });
        }
    }
}
