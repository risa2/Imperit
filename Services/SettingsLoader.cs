namespace Imperit.Services
{
    public interface ISettingsLoader
    {
        State.Settings Settings { get; set; }
    }
    public class SettingsLoader : ISettingsLoader
    {
        readonly Load.Writer<Load.Settings, State.Settings, bool> loader;
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
            loader = new Load.Writer<Load.Settings, State.Settings, bool>(io.Settings, false, Load.Settings.FromSettings);
            settings = loader.LoadOne();
        }
    }
}