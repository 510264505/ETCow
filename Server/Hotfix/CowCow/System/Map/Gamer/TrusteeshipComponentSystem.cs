using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class TrusteeshipComponentStartSystem : StartSystem<TrusteeshipComponent>
    {
        public override void Start(TrusteeshipComponent self)
        {
            self.Start();
        }
    }
    public static class TrusteeshipComponentSystem
    {
        public static async void Start(this TrusteeshipComponent self)
        {
            //Room room = Game.Scene.GetComponent<RoomComponent>().Get(self.GetParent<Gamer>().RoomID);
            //BankerComponent orderController = room.GetComponent<BankerComponent>();
            //Gamer gamer = self.GetParent<Gamer>();
            //bool isStartPlayCard = false;
            //while (true)
            //{
            //    await Game.Scene.GetComponent<TimerComponent>().WaitSecondsAsync(1);
            //    if (self.IsDisposed)
            //    {
            //        return;
            //    }

            //    //自动出牌开关，用于托管延迟开牌
            //    if (gamer.UserID != orderController?.CurrentBankerChairID)
            //    {
            //        continue;
            //    }

            //    ActorMessageSender actorProxy = Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(gamer.InstanceId);
            //    //当还没抢庄时随机抢庄
            //    if (gamer.GetComponent<HandCardComponent>().AccessIdentity == Identity.None)
            //    {
            //        int randomSelect = RandomHelper.RandomNumber(0, 7);
            //    }
            //}
        }
    }
}
