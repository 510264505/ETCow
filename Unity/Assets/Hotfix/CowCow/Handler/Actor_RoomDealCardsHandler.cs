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
            UICowCow_GamerInfoComponent gc = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>().GamerComponent.LocalGamer.GetComponent<UICowCow_GamerInfoComponent>();
            for (int i = 0; i < message.Cards.count; i++)
            {
                gc.SetCard(i, (Sprite)rc.GetAsset(UICowCowAB.CowCow_Texture.StringToAB(), CardHelper.GetCardAssetName(message.Cards[i])));
            }
        }
    }
}
