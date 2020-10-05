using Imperit.State;

namespace Imperit.Pages
{
	public class NewGameModel
	{
		public string OldPassword { get; set; } = "";
		public string NewPassword { get; set; } = "";
		public Settings GetSettings(Settings old)
		{
			return new Settings(old.DebtLimit, old.DefaultInstability, old.DefaultMoney, old.Interest, old.LandColor, old.MountainsColor, old.MountainsWidth, new Password(NewPassword), old.RobotNames, old.SeaColor, old.SoldierTypes);
		}
	}
}
