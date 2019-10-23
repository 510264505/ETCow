using System.Linq;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class Actor_GamerReadyHandler : AMHandler<Actor_CowCowGamerReady>
    {
        protected override void Run(ETModel.Session session, Actor_CowCowGamerReady message)
        {
            UICowCow_GameRoomComponent room = Game.Scene.GetComponent<UIComponent>().Get(UICowCowType.CowCowGameRoom).GetComponent<UICowCow_GameRoomComponent>();
            room.GamerReady(message.SeatIDs.ToArray());
            if (message.IsFullPeople)
            {
                room.ShowHideGrabBanker(true);
            }
        }
    }
}
