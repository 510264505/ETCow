using System.Collections.Generic;

namespace ETModel
{
    public enum RoomState
    {
        None,
        Wait,
        End,
        Game,
    }
    public sealed class Room : Entity
    {
        public readonly Dictionary<long, int> seats = new Dictionary<long, int>(); //椅子号？？
        public readonly List<Gamer> gamers = new List<Gamer>();

        public int RoomID { get; set; }
        public RoomState State { get; set; } = RoomState.None;
        public int Count { get { return seats.Values.Count; } }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            seats.Clear();

            for (int i = 0; i < gamers.Count; i++)
            {
                if (gamers[i] != null)
                {
                    gamers[i].Dispose();
                    gamers[i] = null;
                }
            }
            State = RoomState.None;
        }
    }
}
