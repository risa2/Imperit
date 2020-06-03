namespace Imperit.State
{
    public class Sea : Province
    {
        public Sea(int id, string name, IArmy army)
            : base(id, name, army, new PeasantArmy(0), 0) { }
        protected override Province WithArmy(IArmy army) => new Sea(Id, Name, army);
        public override uint CanMoveTo(Province dest) => !(dest is Land) || dest is Port ? Army.Soldiers : 0;
    }
}