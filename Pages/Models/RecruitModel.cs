using Imperit.State;

namespace Imperit.Pages
{
	public class RecruitModel
	{
		public SoldierType[] SoldierTypes = System.Array.Empty<SoldierType>();
		public IntModel[] Soldiers = System.Array.Empty<IntModel>();
		public bool Borrow { get; set; }
	}
}
