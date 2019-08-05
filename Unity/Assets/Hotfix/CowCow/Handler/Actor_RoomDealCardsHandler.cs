using UnityEngine;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_RoomDealCardsHandler : AMHandler<Actor_CowCowRoomDealCards>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowRoomDealCards message)
        {
            ResourcesComponent rc = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            rc.LoadBundle(UICowCowAB.CowCow_Texture.StringToAB());
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            UICowCow_GamerInfoComponent gic = room.GamerComponent.LocalGamer.GetComponent<UICowCow_GamerInfoComponent>();
            for (int i = 0; i < message.Cards.count; i++)
            {
                gic.SetCard(i, message.Cards[i], (Sprite)rc.GetAsset(UICowCowAB.CowCow_Texture.StringToAB(), CardHelper.GetCardAssetName(message.Cards[i])));
            }
            gic.ShowHidePromptButton(true);
            room.DealCardsGiveAllGamer();
        }
    }
}
