using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class C2MCowCow_GamerReadyHandler : AMRpcHandler<C2M_CowCowGamerReady, M2C_CowCowGamerReady>
    {
        protected override void Run(Session session, C2M_CowCowGamerReady message, Action<M2C_CowCowGamerReady> reply)
        {
            M2C_CowCowGamerReady response = new M2C_CowCowGamerReady();
            try
            {
                Room room = Game.Scene.GetComponent<RoomComponent>().Get(message.RoomID);
                if (room == null)
                {
                    response.Error = ErrorCode.ERR_NotRoomNumberError;
                    reply(response);
                    return;
                }
                Gamer gamer = room.Get(message.SeatID);
                gamer.Status = GamerStatus.Ready; //准备

                Actor_CowCowGamerReady readyInfo = new Actor_CowCowGamerReady();
                readyInfo.SeatIDs = new RepeatedField<int>();
                Dictionary<int, Gamer> gamers = room.GetAll();
                foreach (Gamer g in gamers.Values)
                {
                    if (g.Status == GamerStatus.Ready)
                    {
                        readyInfo.SeatIDs.Add(g.SeatID);
                    }
                }
                reply(response);

                readyInfo.IsFullPeople = readyInfo.SeatIDs.Count == room.PeopleCount;
                if (readyInfo.IsFullPeople)
                {
                    room.CurBureau++;
                    readyInfo.CurBureau = room.CurBureau;
                    UserInfo userInfo = Game.Scene.GetComponent<UserInfoComponent>().Get(room.UserID);
                    DBProxyComponent db = Game.Scene.GetComponent<DBProxyComponent>();
                    BsonDocument bsons0 = new BsonDocument();
                    bsons0["_id"] = room.UserID;
                    BsonDocument bsons1 = new BsonDocument();
                    bsons1["Diamond"] = userInfo.Diamond;
                    db.QueryToUpdate<UserInfo>(bsons0, bsons1).Coroutine();
                }
                //广播准备玩家，所有人准备后，玩家即可开始抢庄
                room.Broadcast(readyInfo);
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
