using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
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

                Actor_CowCowGamerSubmitCardType submits = new Actor_CowCowGamerSubmitCardType();
                submits.SeatIDs = new Google.Protobuf.Collections.RepeatedField<int>();
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
                    Actor_CowCowRoomOpenCardsAndSettlement openCards = new Actor_CowCowRoomOpenCardsAndSettlement();
                    openCards.SmallSettlemntInfo = new Google.Protobuf.Collections.RepeatedField<CowCowSmallSettlementInfo>();
                    foreach (Gamer g in gamers.Values)
                    {
                        CowCowSmallSettlementInfo info = new CowCowSmallSettlementInfo();
                        info.SeatIDs = g.SeatID;
                        info.Cards.AddRange(g.cards);
                        info.CardsType = g.cardType;
                        openCards.SmallSettlemntInfo.Add(info);
                    }
                    room.Broadcast(openCards);
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
    }
}
