using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class C2MCowCow_GamerReadyHandler : AMRpcHandler<C2M_CowCowGamerReady, M2C_CowCowGamerReady>
    {
        private const int CardCount = 5; //每人持牌数
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

                //给各个玩家发牌
                if (readyInfo.SeatIDs.count == room.PeopleCount)
                {
                    List<int> allCards = CowCowDealCardSystem.DealAllCards(CardCount * room.GamerCount);
                    foreach (Gamer g in gamers.Values)
                    {
                        g.Status = GamerStatus.Playing; //正在玩的状态 
                        Actor_CowCowRoomDealCards sendCards = new Actor_CowCowRoomDealCards();
                        sendCards.Cards = new RepeatedField<int>();
                        int index = g.SeatID * CardCount;
                        for (int i = 0; i < CardCount; i++)
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
