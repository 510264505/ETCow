﻿using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_GamerJoinRoomHandler : AMRpcHandler<C2G_CowCowJoinGameRoomGate, G2C_CowCowJoinGameRoomGate>
    {
        protected override async void Run(Session session, C2G_CowCowJoinGameRoomGate message, Action<G2C_CowCowJoinGameRoomGate> reply)
        {
            G2C_CowCowJoinGameRoomGate response = new G2C_CowCowJoinGameRoomGate();
            try
            {
                Log.WriteLine($"Session:{session.Id},userId:{message.UserID}");
                UserInfo userInfo = Game.Scene.GetComponent<UserInfoComponent>().Get(message.UserID);
                Room room = Game.Scene.GetComponent<RoomComponent>().Get(message.RoomID);
                if (room == null)
                {
                    response.Error = ErrorCode.ERR_NotRoomNumberError;
                    response.Message = "房间号不存在";
                    reply(response);
                    return;
                }

                //超过七个人
                if (room.GamerCount >= room.PeopleCount)
                {
                    response.Error = ErrorCode.ERR_RoomPeopleFullError;
                    response.Message = "房间人已满";
                    reply(response);
                    return;
                }

                response.GameName = room.GameName;
                response.Bureau = room.Bureau;
                response.RuleBit = room.RuleBit;
                response.RoomID = room.RoomID;
                response.CurBureau = room.CurBureau;

                Gamer gamer = GamerFactory.Create(message.UserID, session.InstanceId);
                await gamer.AddComponent<MailBoxComponent>().AddLocation();
                gamer.UserID = message.UserID; //玩家账号
                gamer.Name = "闲家" + userInfo.NickName;
                gamer.HeadIcon = userInfo.HeadIcon;
                gamer.Sex = userInfo.Sex;
                gamer.Coin = 0;
                gamer.RoomID = room.RoomID;
                gamer.Status = GamerStatus.Down;
                gamer.IsOffline = false;
                gamer.Identity = Identity.None;
                room.Add(gamer);
                Game.Scene.GetComponent<RoomComponent>().Add(gamer.UserID, room.RoomID);

                Dictionary<int, Gamer> gamers = room.GetAll();
                Actor_CowCowJoinGameRoomGroupSend allGamer = new Actor_CowCowJoinGameRoomGroupSend();
                allGamer.LocalGamerInfo = new GamerInfo();
                allGamer.LocalGamerInfo.Coin = gamer.Coin;
                allGamer.LocalGamerInfo.Name = gamer.Name;
                allGamer.LocalGamerInfo.HeadIcon = gamer.HeadIcon;
                allGamer.LocalGamerInfo.SeatID = gamer.SeatID;
                allGamer.LocalGamerInfo.Sex = gamer.Sex;
                allGamer.LocalGamerInfo.Status = (int)gamer.Status;
                allGamer.LocalGamerInfo.UserID = gamer.UserID;
                allGamer.GamerInfo = new RepeatedField<GamerInfo>();
                foreach (Gamer g in gamers.Values)
                {
                    GamerInfo gamerInfo = new GamerInfo();
                    gamerInfo.Coin = g.Coin;
                    gamerInfo.Name = g.Name;
                    gamerInfo.HeadIcon = g.HeadIcon;
                    gamerInfo.SeatID = g.SeatID;
                    gamerInfo.Sex = g.Sex;
                    gamerInfo.Status = (int)g.Status;
                    gamerInfo.UserID = g.UserID;

                    allGamer.GamerInfo.Add(gamerInfo);
                }

                reply(response);

                room.Broadcast(allGamer);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
