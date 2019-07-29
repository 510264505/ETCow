using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_GamerCreateRoomHandler : AMRpcHandler<C2G_CowCowCreateGameRoomGate, G2C_CowCowCreateGameRoomGate>
    {
        protected override async void Run(Session session, C2G_CowCowCreateGameRoomGate message, Action<G2C_CowCowCreateGameRoomGate> reply)
        {
            G2C_CowCowCreateGameRoomGate response = new G2C_CowCowCreateGameRoomGate();
            try
            {
                UserInfo userInfo = Game.Scene.GetComponent<UserInfoComponent>().Get(message.UserID);
                if (userInfo.Diamond < message.Bureau)
                {
                    response.Error = ErrorCode.ERR_CreateRoomError;
                    response.Message = "房卡不足";
                    reply(response);
                    return;
                }
                userInfo.Diamond -= message.Bureau;

                string roomId = string.Empty;
                while (true)
                {
                    roomId = RandomHelper.RandomNumber(100000, 999999).ToString();
                    if (!Game.Scene.GetComponent<RoomComponent>().IsExist(roomId))
                    {
                        break;
                    }
                }

                Room room = ComponentFactory.Create<Room>();
                room.UserID = message.UserID;
                room.RoomID = roomId;
                room.GameName = message.Name;
                room.Bureau = message.Bureau;
                room.RuleBit = message.RuleBit;
                Game.Scene.GetComponent<RoomComponent>().Add(room);

                Gamer gamer = GamerFactory.Create(message.UserID, session.InstanceId);
                await gamer.AddComponent<MailBoxComponent>().AddLocation();
                gamer.UserID = message.UserID;
                gamer.Name = "房主" + userInfo.NickName;
                gamer.HeadIcon = userInfo.HeadIcon;
                gamer.RoomID = room.RoomID;
                gamer.Status = 1;
                gamer.IsOffline = false;
                gamer.Identity = Identity.None;
                gamer.Coin = 0;
                gamer.Sex = userInfo.Sex;
                room.Add(gamer);

                response.GameName = message.Name;
                response.Bureau = message.Bureau;
                response.RuleBit = message.RuleBit;
                response.RoomID = room.RoomID;
                //根据UserID从数据库拿到该用户信息并返回
                response.GamerInfo = new GamerInfo();
                response.GamerInfo.Name = gamer.Name;
                response.GamerInfo.HeadIcon = gamer.HeadIcon;
                response.GamerInfo.UserID = gamer.UserID; //这个ID用于保存？待定
                response.GamerInfo.SeatID = gamer.SeatID;
                response.GamerInfo.Sex = gamer.Sex;
                response.GamerInfo.Status = gamer.Status;
                response.GamerInfo.Coin = gamer.Coin;

                reply(response);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
