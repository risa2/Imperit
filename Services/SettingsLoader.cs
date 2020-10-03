namespace Imperit.Services
{
	public interface ISettingsLoader
	{
		State.Settings Settings { get; set; }
	}
	public class SettingsLoader : ISettingsLoader
	{
		readonly Load.JsonWriter<Load.JsonSettings, State.Settings, bool> loader;
		State.Settings settings;
		public State.Settings Settings
		{
			get => settings;
			set
			{
				settings = value;
				loader.Save(settings);
			}
		}
		public SettingsLoader(IServiceIO io)
		{
			loader = new Load.JsonWriter<Load.JsonSettings, State.Settings, bool>(io.Settings, false, Load.JsonSettings.From);
			settings = loader.LoadOne();
		}
	}
}