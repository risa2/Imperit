using System;

namespace Imperit.State
{
    public class Port : Land
    {
        public readonly uint Capacity, CanBoard;
        public Port(int id, string name, IArmy army, IArmy defaultArmy, bool isStart, uint earnings, uint capacity, uint boardLimit)
            : base(id, name, army, defaultArmy, isStart, earnings)
        {
            Capacity = capacity;
            CanBoard = boardLimit;
        }
        protected override Province WithArmy(IArmy army) => new Port(Id, Name, army, DefaultArmy, IsStart, Earnings, Capacity, CanBoard);
        protected virtual Port WithCanBoard(uint canBoard) => new Port(Id, Name, Army, DefaultArmy, IsStart, Earnings, Capacity, canBoard);
        public override uint CanMoveTo(Province dest) => dest is Sea ? Math.Min(CanBoard, Army.Soldiers) : base.CanMoveTo(dest);


        private Province baseStartMove(Province dest, IArmy army) => base.StartMove(dest, army);
        public override Province StartMove(Province dest, IArmy army) => WithCanBoard(CanBoard - (dest is Sea ? army.Soldiers : 0)).baseStartMove(dest, army);
        public Port Renew() => WithCanBoard(Capacity);
    }
}