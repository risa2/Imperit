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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<Services.IServiceIO>(s => new Services.ServiceIO(new Load.File("./Files/Settings.json"), new Load.File("./Files/Players.json"), new Load.File("./Files/Provinces.json"), new Load.File("./Files/Actions.json"), new Load.File("./Files/Events.json"), new Load.File("./Files/Active.json"), new Load.File("./Files/Password.txt"), new Load.File("./Files/Graph.json"), new Load.File("./Files/Mountains.json"), new Load.File("./Files/Shapes.json"), new Load.File("Files/Powers.json")));
            services.AddSingleton<Services.IActionWriter, Services.ActionWriter>();
            services.AddSingleton<Services.ISettingsLoader, Services.SettingsLoader>();
            services.AddSingleton<Services.IPlayersLoader, Services.PlayersLoader>();
            services.AddSingleton<Services.IProvincesLoader, Services.ProvincesLoader>();
            services.AddSingleton<Services.IMap, Services.Map>();

            services.AddTransient<Services.IActivePlayer, Services.ActivePlayer>();
            services.AddTransient<Services.INewGame, Services.NewGame>();
            services.AddTransient<Services.IActionReader, Services.ActionReader>();
            services.AddTransient<Services.IPassword, Services.Password>();
            services.AddTransient<Services.IPlayersPowers, Services.PlayersPowers>();
            services.AddTransient<Services.IEndOfTurnManager, Services.EndOfTurnManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
