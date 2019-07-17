using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
    public static class RoomSystem
    {
        /// <summary>
        /// 添加玩家
        /// </summary>
        public static void Add(this Room self, Gamer gamer)
        {
            int seatIndex = self.GetEmptySeat();
        }

        /// <summary>
        /// 获取玩家
        /// </summary>
        public static Gamer Get(this Room self, long id)
        {
            int seatIndex = self.GetGamerSeat(id);
            if (seatIndex >= 0)
            {
                return self.gamers[seatIndex];
            }
            return null;
        }

        public static List<Gamer> GetAll(this Room self)
        {
            return self.gamers;
        }

        /// <summary>
        /// 获取空座位
        /// </summary>
        public static int GetEmptySeat(this Room self)
        {
            for (int i = 0; i < self.gamers.Count; i++)
            {
                if (self.gamers[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取玩家座位
        /// </summary>
        public static int GetGamerSeat(this Room self, long id)
        {
            if (self.seats.TryGetValue(id, out int seatIndex))
            {
                return seatIndex;
            }
            return -1;
        }

        /// <summary>
        /// 移除玩家并返回
        /// </summary>
        public static Gamer Remove(this Room self,long id)
        {
            int seatIndex = self.GetGamerSeat(id);
            if (seatIndex >= 0)
            {
                Gamer gamer = self.gamers[seatIndex];
                self.gamers[seatIndex] = null;
                self.seats.Remove(id);

                gamer.RoomID = string.Empty;
                return gamer;
            }
            return null;
        }

        public static void Broadcast(this Room self, IActorMessage message)
        {
            for (int i = 0; i < self.gamers.Count; i++)
            {
                if (self.gamers[i] == null || self.gamers[i].IsOffline)
                {
                    continue;
                }
                ActorMessageSender actorProxy = self.gamers[i].GetComponent<UnitGateComponent>().GetActorMessageSender();
                actorProxy.Send(message);
            }
        }
    }
}
