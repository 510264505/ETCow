using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
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
                gamer.Status = 2;

                Actor_CowCowGamerReady readyInfo = new Actor_CowCowGamerReady();
                readyInfo.SeatIDs = new Google.Protobuf.Collections.RepeatedField<int>();
                Dictionary<int, Gamer> gamers = room.GetAll();
                foreach (Gamer g in gamers.Values)
                {
                    if (g.Status == 2)
                    {
                        readyInfo.SeatIDs.Add(g.SeatID);
                    }
                }
                reply(response);
                if (readyInfo.SeatIDs.count == room.PeopleCount)
                {
                    //SendCards
                    Actor_CowCowRoomSendCards sendCards = new Actor_CowCowRoomSendCards();
                    int count = 5; //每人持牌数
                    List<int> allCards = CowCowDealCardSystem.DealAllCards(count * room.GamerCount);
                    foreach (Gamer g in gamers.Values)
                    {
                        sendCards.Cards.Clear();
                        int index = g.SeatID * count;
                        for (int i = 0; i < count; i++)
                        {
                            sendCards.Cards.Add(allCards[index + i]);
                        }
                        room.Send(g, sendCards);
                    }
                }
                else
                {
                    room.Broadcast(readyInfo);
                }
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
