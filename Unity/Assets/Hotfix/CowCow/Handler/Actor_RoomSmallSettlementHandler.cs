using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_RoomSmallSettlementHandler : AMHandler<Actor_CowCowRoomOpenCardsAndSettlement>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowRoomOpenCardsAndSettlement message)
        {
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            room.OpenAllGamerHandCard(message.SmallSettlemntInfo.ToArray()).Coroutine();
        }
    }
}
