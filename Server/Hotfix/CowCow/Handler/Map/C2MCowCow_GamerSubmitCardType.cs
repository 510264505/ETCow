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
                    SmallSettlement(gamers, room);
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
            foreach (Gamer g in gamers.Values)
            {
                //坐下
                g.Status = GamerStatus.Down;
                g.IsSubmitHandCards = false;
                CowCowSmallSettlementInfo info = new CowCowSmallSettlementInfo();
                info.Cards = new RepeatedField<int>();
                info.SeatID = g.SeatID;
                info.Cards.AddRange(g.cards);
                info.CardsType = g.cardType;
                info.CowNumber = g.CowNumber;
                openCards.SmallSettlemntInfo.Add(info);
            }
            room.Broadcast(openCards);
        }
    }
}
