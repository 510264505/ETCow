using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

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
                this.ChangeCowCount(gamer, room.CurBankerSeatID == gamer.SeatID, message.CardType, message.CowNumber);

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
                    // 发送大结算
                    if (room.CurBureau == room.Bureau)
                    {
                        Actor_CowCowBigSettlement bs = new Actor_CowCowBigSettlement();
                        bs.Info = new RepeatedField<CowCowBigSettlementInfo>();
                        foreach (Gamer g in gamers.Values)
                        {
                            CowCowBigSettlementInfo info = new CowCowBigSettlementInfo();
                            info.SeatID = g.SeatID;
                            info.Banker = g.BankerCount;
                            info.FiveSmallCow = g.FiveSmallCowCount;
                            info.FiveFlowerCow = g.FiveFlowerCowCount;
                            info.BombCow = g.BombCowCount;
                            info.DoubleCow = g.DoubleCowCount;
                            info.HaveCow = g.HaveCowCount;
                            info.NotCow = g.NotCowCount;
                            info.TotalScore = g.Coin;

                            bs.Info.Add(info);
                        }
                        room.Broadcast(bs);
                    }
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

        private void ChangeCowCount(Gamer gamer, bool isBanker, int cardType, int cowNumber)
        {
            if (isBanker)
            {
                gamer.BankerCount++;
            }
            switch (cardType)
            {
                case 0:
                    gamer.NotCowCount++;
                    break;
                case 1:
                    if (cowNumber == 0)
                    {
                        gamer.DoubleCowCount++;
                    }
                    else
                    {
                        gamer.HaveCowCount++;
                    }
                    break;
                case 2:
                    gamer.FiveFlowerCowCount++;
                    break;
                case 3:
                    gamer.BombCowCount++;
                    break;
                case 4:
                    gamer.FiveSmallCowCount++;
                    break;
            }
        }
    }
}
