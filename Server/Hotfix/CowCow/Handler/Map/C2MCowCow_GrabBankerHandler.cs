using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class C2MCowCow_GrabBankerHandler : AMRpcHandler<C2M_CowCowGrabBanker, M2C_CowCowGrabBanker>
    {
        private const int CardCount = 5; //每人持牌数
        private readonly List<int> seatId = new List<int>();
        protected override void Run(Session session, C2M_CowCowGrabBanker message, Action<M2C_CowCowGrabBanker> reply)
        {
            M2C_CowCowGrabBanker response = new M2C_CowCowGrabBanker();
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
                gamer.GrabBanker = message.Multiple;

                reply(response);

                Dictionary<int, Gamer> gamers = room.GetAll();
                room.grabBankers.Clear();
                foreach (Gamer g in gamers.Values)
                {
                    if (g.GrabBanker != 0)
                    {
                        room.grabBankers.Add(g.GrabBanker);
                    }
                }
                //抢庄后发牌，庄家
                if (room.grabBankers.Count == room.PeopleCount)
                {
                    //判断最大值
                    this.seatId.Clear();
                    int max = room.grabBankers.Max();
                    foreach (Gamer g in gamers.Values)
                    {
                        if (g.GrabBanker == max)
                        {
                            this.seatId.Add(g.SeatID);
                        }
                    }
                    //庄家
                    int seatId = this.seatId[RandomHelper.RandomNumber(this.seatId.Count)];
                    room.CurBankerSeatID = seatId;
                    room.CurMultiple = max;
                    List<int> allCards = CowCowDealCardSystem.DealAllCards(CardCount * room.GamerCount);
                    foreach (Gamer g in gamers.Values)
                    {
                        g.GrabBanker = 0; //初始化
                        g.Status = GamerStatus.Playing; //正在玩的状态 
                        Actor_CowCowRoomDealCards sendCards = new Actor_CowCowRoomDealCards();
                        sendCards.Cards = new RepeatedField<int>();
                        int index = g.SeatID * CardCount;
                        for (int i = 0; i < CardCount; i++)
                        {
                            sendCards.Cards.Add(allCards[index + i]);
                        }
                        sendCards.SeatID = seatId;
                        sendCards.Multiple = max;
                        room.Send(g, sendCards);
                    }
                }
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
