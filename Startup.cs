using Imperit.Load;
using Imperit.Services;
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

			_ = services.AddSingleton<IServiceIO>(s => new ServiceIO(new File("./Files/Settings.json"), new File("./Files/Players.json"), new File("./Files/Provinces.json"), new File("./Files/Actions.json"), new File("./Files/Events.json"), new File("./Files/Active.json"), new File("./Files/Password.txt"), new File("./Files/Graph.json"), new File("./Files/Mountains.json"), new File("./Files/Shapes.json"), new File("./Files/Powers.json"), new File("./Files/Game.json"), new File("./Files/FormerPlayers.json")))
					.AddSingleton<ILoginSession, LoginSession>().AddSingleton<IActionLoader, ActionLoader>()
					.AddSingleton<ISettingsLoader, SettingsLoader>().AddSingleton<IPlayersLoader, PlayersLoader>()
					.AddSingleton<IFormerPlayersLoader, FormerPlayersLoader>()
					.AddSingleton<IProvincesLoader, ProvincesLoader>().AddSingleton<IPowersLoader, PowersLoader>()
					.AddSingleton<IGameLoader, GameLoader>().AddTransient<IActivePlayer, ActivePlayer>()
					.AddTransient<INewGame, NewGame>().AddTransient<IEndOfTurn, EndOfTurn>();
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
