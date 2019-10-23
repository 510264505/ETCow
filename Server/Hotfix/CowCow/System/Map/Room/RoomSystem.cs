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
            gamer.SeatID = self.GetEmptySeat();
            self.gamers.Add(gamer.SeatID, gamer);
        }

        /// <summary>
        /// 获取玩家
        /// </summary>
        public static Gamer Get(this Room self, int seatId)
        {
            if (seatId >= 0)
            {
                return self.gamers[seatId];
            }
            return null;
        }

        public static Dictionary<int, Gamer> GetAll(this Room self)
        {
            return self.gamers;
        }

        /// <summary>
        /// 获取空座位
        /// </summary>
        public static int GetEmptySeat(this Room self)
        {
            int n = 0;
            while (true)
            {
                if (!self.gamers.ContainsKey(n) || self.gamers[n] == null)
                {
                    return n;
                }
                n++;
            }
        }

        /// <summary>
        /// 移除玩家并返回
        /// </summary>
        public static Gamer Remove(this Room self,int seatId)
        {
            if (seatId >= 0)
            {
                Gamer gamer = self.gamers[seatId];
                self.gamers.Remove(seatId);
                gamer.RoomID = string.Empty;
                return gamer;
            }
            return null;
        }

        public static void Send(this Room self, Gamer gamer, IActorMessage message)
        {
            if (gamer == null || gamer.IsOffline)
            {
                return;
            }
            ActorMessageSender actorProxy = gamer.GetComponent<GamerGateComponent>().GetActorMessageSender();
            actorProxy.Send(message);
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        public static void Broadcast(this Room self, IActorMessage message)
        {
            foreach (Gamer gamer in self.gamers.Values)
            {
                if (gamer == null || gamer.IsOffline)
                {
                    continue;
                }
                ActorMessageSender actorProxy = gamer.GetComponent<GamerGateComponent>().GetActorMessageSender();
                actorProxy.Send(message);
            }
        }

        public static void AddDissoltion(this Room self, int seatId, bool isAgree)
        {
            if (!self.dissoltions.ContainsKey(seatId))
            {
                self.dissoltions.Add(seatId, isAgree);
            }
        }

        public static Dictionary<int, bool> GetDissoltions(this Room self)
        {
            return self.dissoltions;
        }

        public static void DissoltionClear(this Room self)
        {
            self.dissoltions.Clear();
        }
    }
}
