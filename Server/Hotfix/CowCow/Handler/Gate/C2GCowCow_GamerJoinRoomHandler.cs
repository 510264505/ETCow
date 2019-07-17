using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2GCowCow_GamerJoinRoomHandler : AMRpcHandler<C2G_CowCowJoinGameRoomGate, G2C_CowCowJoinGameRoomGate>
    {
        protected override void Run(Session session, C2G_CowCowJoinGameRoomGate message, Action<G2C_CowCowJoinGameRoomGate> reply)
        {
            G2C_CowCowJoinGameRoomGate response = new G2C_CowCowJoinGameRoomGate();
            try
            {
                Room room = Game.Scene.GetComponent<RoomComponent>().Get(message.RoomID);

                response.GameName = room.GameName;
                response.Bureau = room.Bureau;
                response.RuleBit = room.RuleBit;
                response.RoomID = room.RoomID;
                response.GamerInfo = new RepeatedField<GamerInfo>();

                List<Gamer> gamers = room.GetAll();
                for (int i = 0; i < gamers.Count; i++)
                {
                    response.GamerInfo[i] = new GamerInfo();
                    response.GamerInfo[i].Coin = 0;
                    response.GamerInfo[i].Diamond = 100;
                    response.GamerInfo[i].HeadIcon = "头像" + i;
                    response.GamerInfo[i].Name = gamers[i].Name;
                }
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
