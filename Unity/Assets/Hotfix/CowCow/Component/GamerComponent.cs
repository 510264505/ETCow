using System.Collections.Generic;
using System.Linq;

namespace ETHotfix
{
    public class GamerComponent : Component
    {
        private readonly Dictionary<long, int> seats = new Dictionary<long, int>();
        private readonly Dictionary<int, Gamer> gamers = new Dictionary<int, Gamer>();

        public Gamer LocalGamer { get; set; }

        public void Add(Gamer gamer, int seatIndex)
        {
            this.gamers[seatIndex] = gamer;
            this.seats[gamer.UserID] = seatIndex;
        }
        public Gamer Get(long id)
        {
            int seatIndex = GetGamerSeat(id);
            if (seatIndex >= 0)
            {
                return gamers[seatIndex];
            }
            return null;
        }
        public Gamer[] GetAll()
        {
            return gamers.Values.ToArray();
        }
        public Dictionary<int, Gamer> GetDictAll()
        {
            return gamers;
        }
        private int GetGamerSeat(long id)
        {
            int seatIndex;
            if (this.seats.TryGetValue(id, out seatIndex))
            {
                return seatIndex;
            }
            return -1;
        }
        public Gamer Remove(long id)
        {
            int seatIndex = GetGamerSeat(id);
            if (seatIndex >= 0)
            {
                Gamer gamer = gamers[seatIndex];
                gamers.Remove(seatIndex);
                seats.Remove(id);
                return gamer;
            }
            return null;
        }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();

            this.LocalGamer = null;
            this.seats.Clear();
            foreach (Gamer gamer in gamers.Values)
            {
                if (gamer != null)
                {
                    gamer.Dispose();
                }
            }
            this.gamers.Clear();
        }
    }
}

