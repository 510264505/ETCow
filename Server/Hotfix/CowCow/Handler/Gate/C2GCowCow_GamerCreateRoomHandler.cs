using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_GamerCreateRoomHandler : AMRpcHandler<C2G_CowCowCreateGameRoomGate, G2C_CowCowCreateGameRoomGate>
    {
        protected override void Run(Session session, C2G_CowCowCreateGameRoomGate message, Action<G2C_CowCowCreateGameRoomGate> reply)
        {
            G2C_CowCowCreateGameRoomGate response = new G2C_CowCowCreateGameRoomGate();
            try
            {
                Room room = ComponentFactory.Create<Room>();
                room.RoomID = RandomHelper.RandomNumber(100000, 999999).ToString();
                room.GameName = message.Name;
                room.Bureau = message.Bureau;
                room.RuleBit = message.RuleBit;
                Game.Scene.GetComponent<RoomComponent>().Add(room);

                response.GameName = message.Name;
                response.Bureau = message.Bureau;
                response.RuleBit = message.RuleBit;
                response.RoomID = room.RoomID;
                //根据UserID从数据库拿到该用户信息并返回
                response.GamerInfo = new GamerInfo();
                response.GamerInfo.Name = "房主";
                response.GamerInfo.HeadIcon = "房主头像base64";
                response.GamerInfo.UserID = message.UserID;
                response.GamerInfo.SeatID = 0;
                response.GamerInfo.Sex = 0;
                response.GamerInfo.Status = 1;
                response.GamerInfo.Coin = 0;
                response.GamerInfo.Diamond = 100; //从数据库拿，暂定
                Gamer gamer = ComponentFactory.Create<Gamer>();
                gamer.ActorID = message.UserID;
                gamer.UserID = message.UserID; //玩家账号
                gamer.Name = response.GamerInfo.Name;
                gamer.SeatID = response.GamerInfo.SeatID;
                gamer.RoomID = room.RoomID;
                gamer.IsReady = false;
                gamer.IsOffline = false;
                gamer.Identity = Identity.None;
                room.Add(gamer);

                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
