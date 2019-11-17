using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_ReConnectHandler : AMRpcHandler<C2G_CowCowReConnect, G2C_CowCowReConnect>
    {
        protected override void Run(Session session, C2G_CowCowReConnect message, Action<G2C_CowCowReConnect> reply)
        {
            G2C_CowCowReConnect response = new G2C_CowCowReConnect();
            try
            {
                Room room = Game.Scene.GetComponent<RoomComponent>().Get(message.UserID);
                if (room == null)
                {
                    response.Error = ErrorCode.ERR_NotRoomNumberError;
                    response.Message = "房间不存在";
                    reply(response);
                    return;
                }
                Dictionary<int, Gamer> gamers = room.GetAll();
                response.Info = new Google.Protobuf.Collections.RepeatedField<GamerReConnectedInfo>();
                foreach (var gamer in gamers.Values)
                {
                    GamerReConnectedInfo g = new GamerReConnectedInfo();
                    g.SeatID = gamer.SeatID;
                    g.Name = gamer.Name;
                    g.HeadIcon = gamer.HeadIcon;
                    g.Cards = new Google.Protobuf.Collections.RepeatedField<int>();
                    for (int i = 0; i < gamer.cards.Count; i++)
                    {
                        g.Cards.Add(gamer.cards[i]);
                    }
                    g.Sex = gamer.Sex;
                    g.Status = (int)gamer.Status;
                    g.UserID = gamer.UserID;
                    g.IsResult = room.State == RoomState.End;
                }
                response.Bureau = room.Bureau;
                response.RuleBit = room.RuleBit;
                response.RoomID = room.RoomID;
                response.GamerInfo = new GamerInfo();
                Gamer gamer1 = room.Get(0);
                response.GamerInfo.Name = gamer1.Name;
                response.GamerInfo.HeadIcon = gamer1.HeadIcon;
                response.GamerInfo.UserID = gamer1.UserID; //这个ID用于保存？待定
                response.GamerInfo.SeatID = gamer1.SeatID;
                response.GamerInfo.Sex = gamer1.Sex;
                response.GamerInfo.Status = (int)gamer1.Status;
                response.GamerInfo.Coin = gamer1.Coin;
                response.People = room.PeopleCount;
                response.CurBureau = room.CurBureau;
                response.GameName = room.GameName;

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
