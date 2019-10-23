using ETModel;
using System.Linq;
using System.Collections.Generic;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_RoomDealCardsHandler : AMHandler<Actor_CowCowRoomDealCards>
    {
        /// <summary>
        /// 发牌，只发给自己
        /// </summary>
        protected override void Run(ETModel.Session session, Actor_CowCowRoomDealCards message)
        {
            ResourcesComponent res = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            UICowCow_GamerInfoComponent gic = room.GamerComponent.LocalGamer.GetComponent<UICowCow_GamerInfoComponent>();
            gic.SetCards(message.Cards.ToArray());
            room.DealCardsGiveAllGamer(message.SeatID, message.Multiple);
            Dictionary<int, Gamer> gamers = room.GamerComponent.GetDictAll();
            foreach (Gamer gamer in gamers.Values)
            {
                gamer.GetComponent<UICowCow_GamerInfoComponent>().ShowHideBankerIcon(false);
            }
            room.GamerComponent.Get(message.SeatID).GetComponent<UICowCow_GamerInfoComponent>().ShowHideBankerIcon(true);
        }
    }
}
