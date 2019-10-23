using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;
using System.Linq;

namespace ETHotfix
{
    [MessageHandler(AppType.Map)]
    public class C2MCowCow_GamerSubmitCardType : AMRpcHandler<C2M_CowCowGamerSubmitCardType, M2C_CowCowGamerSubmitCardType>
    {
        protected override void Run(Session session, C2M_CowCowGamerSubmitCardType message, Action<M2C_CowCowGamerSubmitCardType> reply)
        {
            M2C_CowCowGamerSubmitCardType response = new M2C_CowCowGamerSubmitCardType();
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
                gamer.IsSubmitHandCards = true;
                gamer.MaxValue = message.MaxCard;
                gamer.cardType = message.CardType;
                gamer.FloweColor = message.FlowerColor;
                gamer.CowNumber = message.CowNumber;
                gamer.Multiple = message.Multiple;
                gamer.cards.Clear();
                gamer.cards.AddRange(message.Cards);

                Actor_CowCowGamerSubmitCardType submits = new Actor_CowCowGamerSubmitCardType();
                submits.SeatIDs = new RepeatedField<int>();
                Dictionary<int, Gamer> gamers = room.GetAll();
                foreach (Gamer g in gamers.Values)
                {
                    if (g.IsSubmitHandCards)
                    {
                        submits.SeatIDs.Add(g.SeatID);
                    }
                }
                reply(response);
                if (submits.SeatIDs.count == room.GamerCount)
                {
                    this.SmallSettlement(gamers, room);
                }
                else
                {
                    room.Broadcast(submits);
                }
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
        /// <summary>
        /// 小结算
        /// </summary>
        private void SmallSettlement(Dictionary<int, Gamer> gamers, Room room)
        {
            Actor_CowCowRoomOpenCardsAndSettlement openCards = new Actor_CowCowRoomOpenCardsAndSettlement();
            openCards.SmallSettlemntInfo = new RepeatedField<CowCowSmallSettlementInfo>();
            int bankerCoin = 0;
            //计算各个玩家输赢,判断谁是庄
            foreach (Gamer g in gamers.Values)
            {
                //坐下
                g.Status = GamerStatus.Down;
                g.IsSubmitHandCards = false;
                if (room.CurBankerSeatID != g.SeatID)
                {
                    CowCowSmallSettlementInfo info = new CowCowSmallSettlementInfo();
                    info.Cards = new RepeatedField<int>();
                    info.SeatID = g.SeatID;
                    info.Cards.AddRange(g.cards);
                    info.CardsType = g.cardType;
                    info.CowNumber = g.CowNumber;
                    info.Multiple = g.Multiple;
                    int loseWin = CowComparisonHelper.CalculCoin(gamers[room.CurBankerSeatID], g, room.CurMultiple);
                    g.Coin += loseWin;
                    info.LoseWin = loseWin;
                    bankerCoin += -loseWin;
                    info.BetCoin = g.Coin;
                    openCards.SmallSettlemntInfo.Add(info);
                }
            }
            gamers[room.CurBankerSeatID].Coin += bankerCoin;
            openCards.SmallSettlemntInfo.Add(SetBankerInfo(gamers[room.CurBankerSeatID], bankerCoin));

            room.Broadcast(openCards);
        }

        private CowCowSmallSettlementInfo SetBankerInfo(Gamer g, int bankerCoin)
        {
            CowCowSmallSettlementInfo info = new CowCowSmallSettlementInfo();
            info.Cards = new RepeatedField<int>();
            info.SeatID = g.SeatID;
            info.Cards.AddRange(g.cards);
            info.CardsType = g.cardType;
            info.CowNumber = g.CowNumber;
            info.Multiple = g.Multiple;
            info.LoseWin = bankerCoin;
            info.BetCoin = g.Coin;

            return info;
        }
    }
}
