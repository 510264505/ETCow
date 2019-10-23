using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_DissoltionHandler : AMRpcHandler<C2G_CowCowDissoltion, G2C_CowCowDissoltion>
    {
        protected override void Run(Session session, C2G_CowCowDissoltion message, Action<G2C_CowCowDissoltion> reply)
        {
            G2C_CowCowDissoltion response = new G2C_CowCowDissoltion();
            try
            {
                Room room = Game.Scene.GetComponent<RoomComponent>().Get(message.RoomID);
                room.AddDissoltion(message.SeatID, message.IsAgree);
                Actor_CowCowDissoltion dissMessage = new Actor_CowCowDissoltion();
                dissMessage.Info = new Google.Protobuf.Collections.RepeatedField<DissolutionInfo>();
                Dictionary<int, bool> disss = room.GetDissoltions();
                int agreeCount = 0;
                foreach (var diss in disss)
                {
                    DissolutionInfo info = new DissolutionInfo();
                    info.SeatID = diss.Key;
                    info.IsAgree = diss.Value;
                    dissMessage.Info.Add(info);
                    if (diss.Value)
                    {
                        agreeCount++;
                    }
                }
                // 判断有多少个数量的玩家同意后直接解散，并销毁该房间
                // 当所有人同意，才可以解散，客户端自动倒计时时间然后默认给服务端发送同意(玩家没点，默认同意)
                if (agreeCount >= room.GamerCount)
                {
                    Game.Scene.GetComponent<RoomComponent>().Remove(message.RoomID);
                    dissMessage.IsDiss = true;
                }
                else
                {
                    dissMessage.IsDiss = false;
                }

                reply(response);
                room.Broadcast(dissMessage);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
