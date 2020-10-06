namespace Imperit.Services
{
	public interface ISettingsLoader
	{
		State.Settings Settings { get; }
	}
	public class SettingsLoader : ISettingsLoader
	{
		readonly Load.JsonWriter<Load.JsonSettings, State.Settings, bool> loader;
		public State.Settings Settings { get; }
		public SettingsLoader(IServiceIO io)
		{
			loader = new Load.JsonWriter<Load.JsonSettings, State.Settings, bool>(io.Settings, false, Load.JsonSettings.From);
			Settings = loader.LoadOne();
		}
	}
}